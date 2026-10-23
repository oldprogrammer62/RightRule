using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.DefaultRuleRegistration;

internal sealed class DefaultRulePerContext
{
    public required DefaultRule DefaultRule { get; init;}
    public required string Context { get; init; }
}
