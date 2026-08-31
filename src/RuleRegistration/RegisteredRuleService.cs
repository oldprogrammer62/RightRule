using RightRule.Models;
using RightRule.RuleModels;
using System.Data;

namespace RightRule.RuleRegistration;

internal class RegisteredRuleService : IRegisteredRuleService
{
    private readonly IRegisteredRules _registeredRules;
    private readonly IRuleRegistrationService _ruleRegistrationService;

    public IRegisteredRules RegisteredRules => _registeredRules;

    public RegisteredRuleService(IRuleRegistrationService ruleRegistrationService, IRegisteredRules registeredRules)
    {
        _ruleRegistrationService = ruleRegistrationService;
        _registeredRules = registeredRules;
    }

    public ParsedRule? GetRegisteredRule(string Name)
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        return _registeredRules.Rules.FirstOrDefault(r => string.Equals(r.RuleDefinition.Name, Name, StringComparison.OrdinalIgnoreCase));
    }

    public ParsedRule? GetRegisteredRule(int Number)
    {
        ArgumentNullException.ThrowIfNull(Number, nameof(Number));
        return _registeredRules.Rules.FirstOrDefault(r => r.RuleDefinition.Number == Number);
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _registeredRules.Rules.Where(r => r.RuleDefinition.Context == Context).ToList();
    }

    public List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));

        var parsedRules = _ruleRegistrationService.RegisterRules(rules, OperatorNames, ObjectNames, DatasetNames);
        return RegisteredRules.AddRules(parsedRules);
    }

    public RuleRegistrationResult RegisterRule(DefinedRule rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        var parsedRule = _ruleRegistrationService.RegisterRule(rule, OperatorNames, ObjectNames, DatasetNames);
        if (parsedRule == null)
        {
            return null;
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
