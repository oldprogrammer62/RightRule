using RightRule.RuleModels;
using System.Data;

namespace RightRule.RuleRegistration;

internal class RegisteredRuleService
{
    private readonly RegisteredRules _registeredRules;
    public RegisteredRules RegisteredRules => _registeredRules;

    internal RegisteredRuleService()
    {
        _registeredRules = new RegisteredRules();
    }

    internal ParsedRule? GetRegisteredRule(string Name)
    {
        ArgumentNullException.ThrowIfNull(Name, nameof(Name));
        return _registeredRules.Rules.FirstOrDefault(r => string.Equals(r.Name, Name, StringComparison.OrdinalIgnoreCase));
    }

    internal ParsedRule? GetRegisteredRule(int Number)
    {
        ArgumentNullException.ThrowIfNull(Number, nameof(Number));
        return _registeredRules.Rules.FirstOrDefault(r => r.Number == Number);
    }

    internal List<ParsedRule> GetRules(string Context)
    {
        return _registeredRules.Rules.Where(r => r.Context == Context).ToList();
    }

    internal List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));

        var parsedRules = RuleRegistrationService.RegisterRules(rules, OperatorNames, ObjectNames, DatasetNames);
        RegisteredRules.AddRules(parsedRules);
        return parsedRules;
    }

    internal ParsedRule RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string> ObjectNames, HashSet<string> DatasetNames)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        var parsedRule = RuleRegistrationService.RegisterRule(rule, OperatorNames, ObjectNames, DatasetNames);
        RegisteredRules.AddRule(parsedRule);
        return parsedRule;
    }

    internal bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.RemoveRules(Context);
    }

    internal bool DeRegisterRules()
    {
        return RegisteredRules.RemoveRules();
    }

    internal bool DeRegisterRule(string Name)
    {
        return RegisteredRules.RemoveRule(Name);
    }

    internal bool DeRegisterRule(int Number)
    {
        return RegisteredRules.RemoveRule(Number);
    }
}
