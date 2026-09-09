using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;

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
        var ruleNumbers = RightRuleService?.GetRuleNumbers();
        return ruleNumbers;
    }

    /// <summary>
    /// Returns the set of Rule Names
    /// </summary>
    /// <returns>The list of Rule Names.</returns>
    public List<string>? GetRuleNames()
    {
        var ruleNames = RightRuleService?.GetRuleNames();
        return ruleNames;
    }

    /// <summary>
    /// Returns the set of Rule Contexts
    /// </summary>
    /// <returns>The list of Rule Contexts.</returns>
    public List<string>? GetRuleContexts()
    {
        var contexts = RightRuleService?.GetRuleContexts();
        return contexts;
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        return RightRuleService.GetDefaultRule(Context);
    }

    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule rule)
    {
        if (rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule Registration Failed");
        }
        return RightRuleService.RegisterDefaultRule(rule);
    }


    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Name.
    /// </summary>
    /// <param name="Name"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? GetRegisteredRule(string Name)
    {
        try
        {
            return RightRuleService.GetRegisteredRule(Name);
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
    public ParsedRule? GetRegisteredRule(int Number)
    {
        try
        {
            return RightRuleService.GetRegisteredRule(Number);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules.
    /// </summary>
    public IRegisteredRules GetRegisteredRules()
    {
        try
        {
            return RightRuleService.RegisteredRules;
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
    public List<ParsedRule> GetRegisteredRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        try
        {
            return RightRuleService.GetRules(Context);
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
            return RightRuleService.RegisterRulesJson(this, JsonData);
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
            return RightRuleService.RegisterRules(rules);
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
            return RightRuleService.RegisterRule(rule);
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
            return RightRuleService.DeRegisterRules(Context);
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
            return RightRuleService.DeRegisterRules();
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
            return RightRuleService.DeRegisterRule(Name);
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
            return RightRuleService.DeRegisterRule(Number);
        }
        catch
        {
            throw;
        }
    }
}
