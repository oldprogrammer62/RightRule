using RightRule.Models.RuleModels;
using System.Data;

namespace RightRule.RegistrationServices.RuleRegistration;

internal class RuleService : IRuleService
{
    private readonly IRuleRegistrationService _ruleRegistrationService;

    private readonly IRegisteredRules _registeredRules;
    public IRegisteredRules RegisteredRules => _registeredRules;

    public RuleService(IRuleRegistrationService ruleRegistrationService, IRegisteredRules registeredRules)
    {
        _ruleRegistrationService = ruleRegistrationService;
        _registeredRules = registeredRules;
    }

    public ParsedRule? GetRule(string Name)
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        return _registeredRules.Rules.FirstOrDefault(r => string.Equals(r.Name, Name, StringComparison.OrdinalIgnoreCase));
    }

    public ParsedRule? GetRule(int Number)
    {
        return _registeredRules.Rules.FirstOrDefault(r => r.Number == Number);
    }

    public List<ParsedRule> GetRules()
    {
        return _registeredRules.Rules;
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _registeredRules.Rules.Where(r => r.Context == Context).ToList();
    }

    public List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));

        var parsedRules = _ruleRegistrationService.RegisterRules(rules, OperatorNames);
        return RegisteredRules.AddRules(parsedRules);
    }

    public RuleRegistrationResult RegisterRule(DefinedRule rule, HashSet<string> OperatorNames)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        var parsedRule = _ruleRegistrationService.RegisterRule(rule, OperatorNames);
        if (parsedRule == null)
        {
            return new RuleRegistrationResult()
            {
                Message = "Rule Failed to be Parsed",
                RegisteredRule = null,
                Status = RegistrationStatus.FAILURE
            };
        }
        return RegisteredRules.AddRule(parsedRule);
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

    public bool DeRegisterRule(string Name)
    {
        return RegisteredRules.RemoveRule(Name);
    }

    public bool DeRegisterRule(int Number)
    {
        return RegisteredRules.RemoveRule(Number);
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
