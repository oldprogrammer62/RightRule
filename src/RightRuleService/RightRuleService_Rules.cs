using BeSoft;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.Models.RuleModels;
using RightRule.Loaders.RuleLoader;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule rule)
    {
        if (rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed");
        }
        var result = DefaultRuleRegister.SetDefaultRule(rule);
        if (!result)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed", rule);
        }
        return DefaultRuleRegistrationResult.Success("Default Rule registration success", rule);
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        return DefaultRuleRegister.GetDefaultRule(Context);
    }

    public List<(int Number, string Name)>? GetRuleNumbers()
    {
        return RegisteredRuleService.GetRuleNumbers();
    }

    public List<string>? GetRuleNames()
    {
        return RegisteredRuleService.GetRuleNames();
    }

    public List<string>? GetRuleContexts()
    {
        return RegisteredRuleService.GetRuleContexts();
    }

    public ParsedRule? GetRegisteredRule(string Name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name, nameof(Name));
        return RegisteredRuleService.GetRule(Name);
    }

    public ParsedRule? GetRegisteredRule(int Number)
    {
        return RegisteredRuleService.GetRule(Number);
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return RegisteredRuleService.GetRules(Context);
    }

    public List<RuleRegistrationResult> RegisterRulesJson(IRightRule rightRule, string JsonData)
    {
        var ruleDefn = RuleLoaderService.LoadRulesFromJson(rightRule, JsonData);
        if (ruleDefn == null)
        {
            return [RuleRegistrationResult.Failed("Rule Definitions not build from Json")];
        }
        return rightRule.RegisterRules(ruleDefn);
    }

    /// <summary>
    /// Registers a collection of rules with the service, parsing them and making them available for application.
    /// </summary>
    /// <param name="rules">The rule definitions to register.</param>
    /// <returns>The parsed rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException">Thrown when any of the rules contain a syntax error.</exception>
    public List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));

        var operatorNames = OperatorsService.OperatorNames;
        return RegisteredRuleService.RegisterRules(rules, operatorNames);
    }

    /// <summary>
    /// Registers a single rule with the service, parsing it and making it available for application.
    /// </summary>
    /// <param name="rule">The rule definition to register.</param>
    /// <returns>The parsed rule.</returns>
    /// <exception cref="FormatException">Thrown when the rule contains a syntax error.</exception>
    public RuleRegistrationResult RegisterRule(DefinedRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));

        var operatorNames = OperatorsService.OperatorNames;
        return RegisteredRuleService.RegisterRule(rule, operatorNames);
    }

    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return RegisteredRuleService.DeRegisterRules(Context);
    }

    public bool DeRegisterRules()
    {
        return RegisteredRuleService.DeRegisterRules();
    }

    public bool DeRegisterRule(string Name)
    {
        return RegisteredRuleService.DeRegisterRule(Name);
    }

    public bool DeRegisterRule(int Number)
    {
        return RegisteredRuleService.DeRegisterRule(Number);
    }
}
