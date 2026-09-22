using BeSoft;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.Models.RuleModels;
using RightRule.Loaders.RuleLoader;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule)
    {
        if (Rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed");
        }
        var result = DefaultRuleRegister.SetDefaultRule(Rule);
        if (!result)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed", Rule);
        }
        return DefaultRuleRegistrationResult.Success("Default Rule registration success", Rule);
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        return DefaultRuleRegister.GetDefaultRule(Context);
    }

    public List<string>? GetRuleContexts()
    {
        return RuleRegistrationService.GetRuleContexts();
    }

    public List<(int Number, string Name)>? GetRuleNumbers(string Context)
    {
        return RuleRegistrationService.GetRuleNumbers(Context);
    }

    public List<string>? GetRuleNames(string Context)
    {
        return RuleRegistrationService.GetRuleNames(Context);
    }

    public ParsedRule? GetRegisteredRule(string Context, string RuleName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(RuleName, nameof(RuleName));
        return RuleRegistrationService.GetRule(Context, RuleName);
    }

    public ParsedRule? GetRegisteredRule(string Context, int RuleNumber)
    {
        return RuleRegistrationService.GetRule(Context, RuleNumber);
    }

    public List<ParsedRule> GetRegisteredRules(string Context)
    {
        return RuleRegistrationService.GetRules(Context);
    }

    public List<RuleRegistrationResult> RegisterRulesJson(IRightRule RightRule, string JsonData)
    {
        var ruleDefn = RuleLoaderService.LoadRulesFromJson(RightRule, JsonData);
        if (ruleDefn == null)
        {
            return [RuleRegistrationResult.Failed("Rule Definitions not build from Json")];
        }
        return RightRule.RegisterRules(ruleDefn);
    }

    /// <summary>
    /// Registers a collection of rules with the service, parsing them and making them available for application.
    /// </summary>
    /// <param name="Rules">The rule definitions to register.</param>
    /// <returns>The parsed rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException">Thrown when any of the rules contain a syntax error.</exception>
    public List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules)
    {
        ArgumentNullException.ThrowIfNull(Rules, nameof(Rules));
        return RuleRegistrationService.RegisterRules(Rules);
    }

    /// <summary>
    /// Registers a single rule with the service, parsing it and making it available for application.
    /// </summary>
    /// <param name="Rule">The rule definition to register.</param>
    /// <returns>The parsed rule.</returns>
    /// <exception cref="FormatException">Thrown when the rule contains a syntax error.</exception>
    public RuleRegistrationResult RegisterRule(DefinedRule Rule)
    {
        ArgumentNullException.ThrowIfNull(Rule, nameof(Rule));
        return RuleRegistrationService.RegisterRule(Rule);
    }

    public bool DeRegisterRules()
    {
        return RuleRegistrationService.DeRegisterRules();
    }

    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return RuleRegistrationService.DeRegisterRules(Context);
    }

    public bool DeRegisterRule(string Context, string RuleName)
    {
        return RuleRegistrationService.DeRegisterRule(Context, RuleName);
    }

    public bool DeRegisterRule(string Context, int RuleNumber)
    {
        return RuleRegistrationService.DeRegisterRule(Context, RuleNumber);
    }
}
