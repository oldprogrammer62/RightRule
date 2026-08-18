using RightRule.RuleModels;
using System.Data;

namespace RightRule.RuleRegistration;

internal class RegisteredRuleService : IRegisteredRuleService
{
    private readonly IRegisteredRules _registeredRules;
    public IRegisteredRules RegisteredRules => _registeredRules;

    public RegisteredRuleService(IRegisteredRules registeredRules)
    {
        _registeredRules = registeredRules;
    }

    public ParsedRule? GetRegisteredRule(string Name)
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        return _registeredRules.Rules.FirstOrDefault(r => string.Equals(r.Name, Name, StringComparison.OrdinalIgnoreCase));
    }

    public ParsedRule? GetRegisteredRule(int Number)
    {
        ArgumentNullException.ThrowIfNull(Number, nameof(Number));
        return _registeredRules.Rules.FirstOrDefault(r => r.Number == Number);
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _registeredRules.Rules.Where(r => r.Context == Context).ToList();
    }

    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));

        var parsedRules = RuleRegistrationService.RegisterRules(rules, OperatorNames, ObjectNames, DatasetNames);
        RegisteredRules.AddRules(parsedRules);
        return parsedRules;
    }

    public ParsedRule? RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        var parsedRule = RuleRegistrationService.RegisterRule(rule, OperatorNames, ObjectNames, DatasetNames);
        if (parsedRule == null)
        {
            return null;
        }
        RegisteredRules.AddRule(parsedRule);
        return parsedRule;
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
}
