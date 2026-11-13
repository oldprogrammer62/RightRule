using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    public DefaultRule GetDefaultRule(string Context)
    {
        return _rightRuleService.GetDefaultRule(Context);
    }

    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule)
    {
        if (Rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule Registration Failed");
        }
        return _rightRuleService.RegisterDefaultRule(Rule);
    }

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
