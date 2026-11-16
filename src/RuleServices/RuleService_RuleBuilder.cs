using RightRule.RuleServices.RuleBuilders.RightRuleBuilder;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    /// <summary>
    /// Build an enhanced Rule Definition
    /// Based on Interface to force fiuelds to be applied to the rule defn in order
    /// Entry point kicks off the strict compile-time interface chain
    /// </summary>
    public INumberStep BuildRule()
    {
        return RuleBuilder.Create();
    }
}
