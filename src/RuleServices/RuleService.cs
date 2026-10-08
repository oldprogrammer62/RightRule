using RightRule.Models;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleParsingServices;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;
using System.Data;

namespace RightRule.RuleServices;

internal class RuleService : IRuleService
{
    private readonly IParseRuleService _parseRuleService;

    private readonly IRegisteredRules _registeredRules;
    public IRegisteredRules RegisteredRules => _registeredRules;

    public RuleService(IParseRuleService ParseRuleService, IRegisteredRules RegisteredRules)
    {
        _parseRuleService = ParseRuleService;
        _registeredRules = RegisteredRules;
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

        var parsedRules = _parseRuleService.ParseRules(Rules);
        return RegisteredRules.AddRules(parsedRules);
    }

    public RuleRegistrationResult RegisterRule(DefinedRule Rule)
    {
        ArgumentNullException.ThrowIfNull(Rule, nameof(Rule));
        var parsedRule = _parseRuleService.ParseRule(Rule);
        if (parsedRule == null)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = null,
                Message = "Rule Failed to be Parsed",
                Status = RightRuleStatusEnum.FAILURE
            };
        }
        if (parsedRule.Status == RightRuleStatusEnum.FAILURE)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = parsedRule,
                Message = parsedRule.Message ?? "Registration failed, reason unknown!",
                Status = RightRuleStatusEnum.FAILURE
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
}
