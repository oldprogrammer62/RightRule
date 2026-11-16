using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    public List<string>? GetRuleContexts()
    {
        return RegisteredRules.GetRuleContexts();
    }

    public List<(int Number, string Name)>? GetRuleNumbers(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.GetRuleNumbers(Context);
    }

    public List<string>? GetRuleNames(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.GetRuleNames(Context);
    }

    public ParsedRule? GetRegisteredRule(string Context, string RuleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(RuleName, nameof(RuleName));

        return _registeredRules.Rules.FirstOrDefault(r =>
        r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)
        &&
        string.Equals(r.Name, RuleName, StringComparison.OrdinalIgnoreCase));
    }

    public ParsedRule? GetRegisteredRule(string Context, int RuleNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return _registeredRules.Rules.FirstOrDefault(r =>
        r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)
        &&
        r.Number == RuleNumber);
    }

    public List<ParsedRule> GetRegisteredRules()
    {
        return _registeredRules.Rules;
    }

    public List<ParsedRule> GetRegisteredRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return _registeredRules.Rules.Where(r => r.Context.Equals(Context, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
