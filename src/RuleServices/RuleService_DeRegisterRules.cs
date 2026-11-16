namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
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
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(RuleName, nameof(RuleName));

        return RegisteredRules.RemoveRule(Context, RuleName);
    }

    public bool DeRegisterRule(string Context, int RuleNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return RegisteredRules.RemoveRule(Context, RuleNumber);
    }
}
