using Microsoft.Extensions.Logging;
using RightRule.Models;
using RightRule.RuleServices.RuleLoader;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleParsingService;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;
using RightRule.RuleServices.RuleSerializer;
using RightRule.RuleServices.RuleValidation;
using System.ComponentModel;
using System.Data;

namespace RightRule.RuleServices;

internal sealed class RuleService : IRuleService
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly RightRuleOptions _options;
    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly IRuleValidationService _ruleValidationService;
    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly IParseRuleService _parseRuleService;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly IRegisteredRules _registeredRules;
    private readonly IRuleLoaderService _ruleLoaderService;
    private readonly IRuleSerializerService _ruleSerializerService;
    private readonly ILogger<RuleService> _logger;

    public IRegisteredRules RegisteredRules => _registeredRules;

    public RuleService(RightRuleOptions options, IRuleValidationService RuleValidationService, IParseRuleService ParseRuleService, IRegisteredRules RegisteredRules, IRuleLoaderService ruleLoaderService, IRuleSerializerService ruleSerializerService, ILogger<RuleService> logger)
    {
        _logger = logger;

        _options = options;
        _registeredRules = RegisteredRules;
        _ruleLoaderService = ruleLoaderService;
        _ruleSerializerService = ruleSerializerService;
        _ruleValidationService = RuleValidationService;
        _parseRuleService = ParseRuleService;
    }

    public string SerializeRules()
    {
        var rules = GetRules();
        if (rules == null || rules.Count == 0)
        {
            return string.Empty;
        }
        return _ruleSerializerService.SerializeRules(rules);
    }

    public string SerializeRules(string Context)
    {
        var rules = GetRules(Context);
        if (rules == null || rules.Count == 0)
        {
            return string.Empty;
        }
        return _ruleSerializerService.SerializeRules(rules);
    }

    public List<RuleRegistrationResult> LoadRules(string JsonData)
    {
        var ruleDef = _ruleLoaderService.LoadRules(JsonData);
        if (ruleDef == null)
        {
            return [];
        }
        return RegisterRules(ruleDef);
    }

    public ParsedRule? GetRule(string Context, string RuleName)
    {
        ArgumentNullException.ThrowIfNull(RuleName, nameof(RuleName));
        return _registeredRules.Rules.FirstOrDefault(r =>
        r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)
        &&
        string.Equals(r.Name, RuleName, StringComparison.OrdinalIgnoreCase));
    }

    public ParsedRule? GetRule(string Context, int RuleNumber)
    {
        return _registeredRules.Rules.FirstOrDefault(r => 
        r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)
        &&
        r.Number == RuleNumber);
    }

    public List<ParsedRule> GetRules()
    {
        return _registeredRules.Rules;
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _registeredRules.Rules.Where(r => r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules)
    {
        ArgumentNullException.ThrowIfNull(Rules, nameof(Rules));

        List<RuleRegistrationResult> registeredRules = [];
        foreach (var rule in Rules)
        {
            var ruleRegistration = RegisterRule(rule);
            if (ruleRegistration.Status == RightRuleStatusEnum.SUCCESS)
            {
                registeredRules.Add(ruleRegistration);
                continue;
            }
            registeredRules.Add(ruleRegistration);
        }
        return registeredRules;
    }

    public RuleRegistrationResult RegisterRule(DefinedRule Rule)
    {
        ArgumentNullException.ThrowIfNull(Rule, nameof(Rule));

        var ruleValidationResult = ValidateRule(Rule);
        if (ruleValidationResult.Valid == RightRuleStatusEnum.ERROR)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError("Rule: {number} - {name} not registered. {message}", Rule.Number, Rule.Name, ruleValidationResult.Message);
            }
            return new RuleRegistrationResult()
            {
                RegisteredRule = null,
                Message = ruleValidationResult.Message,
                Status = ruleValidationResult.Valid
            };
        }
        var parsedRule = ParseRule(Rule);
        if (parsedRule == null)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = null,
                Message = "Error: Rule Failed to be Parsed",
                Status = RightRuleStatusEnum.ERROR
            };
        }
        if (parsedRule.Status == RightRuleStatusEnum.SUCCESS)
        {
            return RegisteredRules.AddRule(parsedRule);
        }
        if (parsedRule.Status == RightRuleStatusEnum.DEFAULT)
        {
            return RegisteredRules.AddRule(parsedRule);
        }
        if (parsedRule.Status == RightRuleStatusEnum.FAILURE)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = parsedRule,
                Message = parsedRule.Message ?? "Registration failed, reason unknown!",
                Status = parsedRule.Status
            };
        }
        if (parsedRule.Status == RightRuleStatusEnum.ERROR)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = parsedRule,
                Message = parsedRule.Message ?? "Error: Registration failed, reason unknown!",
                Status = parsedRule.Status
            };
        }
        return new RuleRegistrationResult()
        {
            RegisteredRule = null,
            Message = "Rule Failed to register",
            Status = RightRuleStatusEnum.FAILURE
        };
    }

    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.RemoveRules(Context);
    }

    public bool DeRegisterRules()
    {
        return RegisteredRules.RemoveRules();
    }

    public bool DeRegisterRule(string Context, string RuleName)
    {
        return RegisteredRules.RemoveRule(Context, RuleName);
    }

    public bool DeRegisterRule(string Context, int RuleNumber)
    {
        return RegisteredRules.RemoveRule(Context, RuleNumber);
    }

    public List<string>? GetRuleContexts()
    {
        return RegisteredRules.GetRuleContexts();
    }

    public List<(int Number, string Name)>? GetRuleNumbers(string Context)
    {
        return RegisteredRules.GetRuleNumbers(Context);
    }

    public List<string>? GetRuleNames(string Context)
    {
        return RegisteredRules.GetRuleNames(Context);
    }

    private RuleValidationResult ValidateRule(DefinedRule Rule)
    {
        return _ruleValidationService.ValidateRule(Rule);
    }

    private ParsedRule? ParseRule(DefinedRule Rule)
    {
        var diagnostics = _options.Diagnostics;
        return _parseRuleService.ParseRule(Rule, diagnostics);
    }
}
