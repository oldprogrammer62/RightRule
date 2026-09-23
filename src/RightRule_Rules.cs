using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;

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
    public List<(int Number, string Name)>? GetRuleNumbers(string Context)
    {
        var ruleNumbers = RightRuleService?.GetRuleNumbers(Context);
        return ruleNumbers;
    }

    /// <summary>
    /// Returns the set of Rule Names
    /// </summary>
    /// <returns>The list of Rule Names.</returns>
    public List<string>? GetRuleNames(string Context)
    {
        var ruleNames = RightRuleService?.GetRuleNames(Context);
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

    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule)
    {
        if (Rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule Registration Failed");
        }
        return RightRuleService.RegisterDefaultRule(Rule);
    }


    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Name.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleName"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? GetRegisteredRule(string Context, string RuleName)
    {
        try
        {
            return RightRuleService.GetRegisteredRule(Context, RuleName);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Number.
    /// </summary>
    /// <param name="RuleNumber"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? GetRegisteredRule(string Context, int RuleNumber)
    {
        try
        {
            return RightRuleService.GetRegisteredRule(Context, RuleNumber);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules.
    /// </summary>
    public List<ParsedRule> GetRegisteredRules()
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
            return RightRuleService.GetRegisteredRules(Context);
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
    /// <param name="Rules">The Rule definitions to register.</param>
    /// <returns>The set of Parsed Rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules)
    {
        ArgumentNullException.ThrowIfNull(Rules, nameof(Rules));
        try
        {
            return RightRuleService.RegisterRules(Rules);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a single Rule with the RightRule engine.
    /// </summary>
    /// <param name="Rule">The Rule definition to register.</param>
    /// <returns>The Parsed Rule.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public RuleRegistrationResult RegisterRule(DefinedRule Rule)
    {
        ArgumentNullException.ThrowIfNull(Rule, nameof(Rule));
        try
        {
            return RightRuleService.RegisterRule(Rule);
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
    /// <param name="Context"></param>
    /// <param name="RuleName">The Name of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRule(string Context, string RuleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(RuleName, nameof(RuleName));
        try
        {
            return RightRuleService.DeRegisterRule(Context, RuleName);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleNumber">The Number of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRule(string Context, int RuleNumber)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(RuleNumber.ToString(), nameof(RuleNumber));
        try
        {
            return RightRuleService.DeRegisterRule(Context, RuleNumber);
        }
        catch
        {
            throw;
        }
    }
}
