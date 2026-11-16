namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    public string SerializeRules()
    {
        var rules = GetRegisteredRules();
        if (rules == null || rules.Count == 0)
        {
            return string.Empty;
        }
        return _ruleSerializerService.SerializeRules(rules);
    }

    public string SerializeRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rules = GetRegisteredRules(Context);
        if (rules == null || rules.Count == 0)
        {
            return string.Empty;
        }
        return _ruleSerializerService.SerializeRules(rules);
    }
}
