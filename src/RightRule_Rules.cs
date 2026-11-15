using RightRule.RuleServices.RuleModels;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    /// <summary>
    /// Returns the set of Registered (Parsed) Rules.
    /// </summary>
    public List<ParsedRule> GetRegisteredRules()
    {
        try
        {
            return _rightRuleService.RegisteredRules;
        }
        catch
        {
            throw;
        }
    }
}
