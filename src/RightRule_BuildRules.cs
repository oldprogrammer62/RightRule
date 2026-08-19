using RightRule.RuleBuilder;
using RightRule.RuleModels;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    /// <summary>
    /// Build an enhanced Rule Definition
    /// Based on Interface to force fiuelds to be applied to the rule defn in order
    /// Entry point kicks off the strict compile-time interface chain
    /// </summary>
    public INumberStep BuildRuleDefinition()
    {
        return RuleBuilder.Create();
    }
}
