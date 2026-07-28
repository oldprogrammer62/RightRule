using System.Data;
using System.Xml.Linq;
namespace RightRule.Rules;

internal class RegisteredRuleService : IRegisteredRuleService
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

    internal async Task<List<ParsedRule>> RegisterRulesAsync(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));

        var parsedRules = await RuleRegistrationService.ParseRulesAsync(rules, OperatorNames).ConfigureAwait(false);
        RegisteredRules.AddRules(parsedRules);
        return parsedRules;
    }

    internal async Task<ParsedRule> RegisterRuleAsync(RuleDefinition rule, HashSet<string> OperatorNames)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        var parsedRule = await RuleRegistrationService.ParseRuleAsync(rule, OperatorNames).ConfigureAwait(false);
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
