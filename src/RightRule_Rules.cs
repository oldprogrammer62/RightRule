using RightRule.RuleModels;
using RightRule.RuleRegistration.DefaultRuleRegistration;
using RightRule.RuleRegistration.LogicRules;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{

    /// <summary>
    /// Returns the set of Rule Numbers
    /// </summary>
    /// <returns>The list of Rule Numbers (with Names).</returns>
    public List<(int Number, string Name)>? GetRuleNumbers()
    {
        var ruleNumbers = _rightRuleService?.GetRuleNumbers();
        return ruleNumbers;
    }

    /// <summary>
    /// Returns the set of Rule Names
    /// </summary>
    /// <returns>The list of Rule Names.</returns>
    public List<string>? GetRuleNames()
    {
        var ruleNames = _rightRuleService?.GetRuleNames();
        return ruleNames;
    }

    /// <summary>
    /// Returns the set of Rule Contexts
    /// </summary>
    /// <returns>The list of Rule Contexts.</returns>
    public List<string>? GetRuleContexts()
    {
        var contexts = _rightRuleService?.GetRuleContexts();
        return contexts;
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        return _rightRuleService.GetDefaultRule(Context);
    }

    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule rule)
    {
        if (rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule Registration Failed");
        }
        return _rightRuleService.RegisterDefaultRule(rule);
    }


    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Name.
    /// </summary>
    /// <param name="Name"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? RegisteredRule(string Name)
    {
        try
        {
            return _rightRuleService.GetRegisteredRule(Name);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Number.
    /// </summary>
    /// <param name="Number"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? RegisteredRule(int Number)
    {
        try
        {
            return _rightRuleService.GetRegisteredRule(Number);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules.
    /// </summary>
    public IRegisteredRules RegisteredRules()
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

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    /// <exception cref="ArgumentException"></exception>
    public List<ParsedRule> RegisteredRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        try
        {
            return _rightRuleService.GetRules(Context);
        }
        catch
        {
            throw;
        }
    }

    public List<RuleRegistrationResult> RegisterRulesJson(string JsonData)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(JsonData, nameof(JsonData));
        try
        {
            return _rightRuleService.RegisterRulesJson(this, JsonData);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a set of Rules with the RightRule engine.
    /// </summary>
    /// <param name="rules">The Rule definitions to register.</param>
    /// <returns>The set of Parsed Rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        try
        {
            return _rightRuleService.RegisterRules(rules);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a single Rule with the RightRule engine.
    /// </summary>
    /// <param name="rule">The Rule definition to register.</param>
    /// <returns>The Parsed Rule.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public RuleRegistrationResult RegisterRule(DefinedRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        try
        {
            return _rightRuleService.RegisterRule(rule);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregisters all Rules associated with the specified Context from the RightRule engine.
    /// </summary>
    /// <param name="Context">The Context for which to deregister rules.</param>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        try
        {
            return _rightRuleService.DeRegisterRules(Context);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregisters all Rules from the RightRule engine.
    /// </summary>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    public bool DeRegisterRules()
    {
        try
        {
            return _rightRuleService.DeRegisterRules();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Name">The Name of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRule(string Name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name, nameof(Name));
        try
        {
            return _rightRuleService.DeRegisterRule(Name);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Number">The Number of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRule(int Number)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Number.ToString(), nameof(Number));
        try
        {
            return _rightRuleService.DeRegisterRule(Number);
        }
        catch
        {
            throw;
        }
    }
}
