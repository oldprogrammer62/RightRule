using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;
using System.Data;

namespace RightRule.RegistrationServices.RuleRegistration;

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

    public ParsedRule? GetRule(string RuleName)
    {
        ArgumentNullException.ThrowIfNull(RuleName, nameof(RuleName));
        return _registeredRules.Rules.FirstOrDefault(r => string.Equals(r.Name, RuleName, StringComparison.OrdinalIgnoreCase));
    }

    public ParsedRule? GetRule(int RuleNumber)
    {
        return _registeredRules.Rules.FirstOrDefault(r => r.Number == RuleNumber);
    }

    public List<ParsedRule> GetRules()
    {
        return _registeredRules.Rules;
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _registeredRules.Rules.Where(r => r.Context == Context).ToList();
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
                Status = RegistrationStatusEnum.FAILURE
            };
        }
        if (parsedRule.Status == RegistrationStatusEnum.FAILURE)
        {
            return new RuleRegistrationResult()
            {
                RegisteredRule = parsedRule,
                Message = parsedRule.Message ?? "Registration failed, reason unknown!",
                Status = RegistrationStatusEnum.FAILURE
            };
        }
        if (parsedRule.Status == RegistrationStatusEnum.SUCCESS)
        {
            return RegisteredRules.AddRule(parsedRule);
        }
        if (parsedRule.Status == RegistrationStatusEnum.DEFAULT)
        {
            return RegisteredRules.AddRule(parsedRule);
        }
        return new RuleRegistrationResult()
        {
            RegisteredRule = null,
            Message = "Rule Failed to register",
            Status = RegistrationStatusEnum.FAILURE
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

    public bool DeRegisterRule(string RuleName)
    {
        return RegisteredRules.RemoveRule(RuleName);
    }

    public bool DeRegisterRule(int RuleNumber)
    {
        return RegisteredRules.RemoveRule(RuleNumber);
    }

    public List<(int Number, string Name)>? GetRuleNumbers()
    {
        return RegisteredRules.GetRuleNumbers();
    }

    public List<string>? GetRuleNames()
    {
        return RegisteredRules.GetRuleNames();
    }

    public List<string>? GetRuleContexts()
    {
        return RegisteredRules.GetRuleContexts();
    }
}
