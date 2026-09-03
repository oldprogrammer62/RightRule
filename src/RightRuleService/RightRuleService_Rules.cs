using RightRule.RuleModels;
using BeSoft;
using RightRule.RuleLoaders;
using RightRule.RuleRegistration.LogicRules;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    public List<(int Number, string Name)>? GetRuleNumbers()
    {
        return _registerRuleService.GetRuleNumbers();
    }

    public List<string>? GetRuleNames()
    {
        return _registerRuleService.GetRuleNames();
    }

    public List<string>? GetRuleContexts()
    {
        return _registerRuleService.GetRuleContexts();
    }

    public ParsedRule? GetRegisteredRule(string Name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name, nameof(Name));
        return _registerRuleService.GetRegisteredRule(Name);
    }

    public ParsedRule? GetRegisteredRule(int Number)
    {
        return _registerRuleService.GetRegisteredRule(Number);
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _registerRuleService.GetRules(Context);
    }

    public List<RuleRegistrationResult> RegisterRulesJson(IRightRule rightRule, string JsonData)
    {
        var ruleDefn = RuleLoaderService.LoadRulesFromJson(rightRule, JsonData);
        if (ruleDefn == null)
        {
            return [RuleRegistrationResult.Failed("Rule Definitions not build from Json", null)];
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

        var operatorNames = _operatorsService.OperatorNames;
        var objectNames = _registeredDataObjects?.ObjectNames;
        var datasetNames = _registeredDataSets?.DatasetNames;
        return _registerRuleService.RegisterRules(rules, operatorNames, objectNames, datasetNames);
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

        var operatorNames = _operatorsService.OperatorNames;

        var objectNames = GetObjectNames();
        var datasetNames = GetDatasetNames();

        return _registerRuleService.RegisterRule(rule, operatorNames, objectNames, datasetNames);
    }

    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return _registerRuleService.DeRegisterRules(Context);
    }

    public bool DeRegisterRules()
    {
        return _registerRuleService.DeRegisterRules();
    }

    public bool DeRegisterRule(string Name)
    {
        return _registerRuleService.DeRegisterRule(Name);
    }

    public bool DeRegisterRule(int Number)
    {
        return _registerRuleService.DeRegisterRule(Number);
    }

    private HashSet<string> GetObjectNames()
    {
        if (_registeredDataObjects == null)
        {
            return [];
        }
        return _registeredDataObjects.ObjectNames;
    }

    private HashSet<string> GetDatasetNames()
    {
        if (_registeredDataSets == null)
        {
            return [];
        }
        return _registeredDataSets.DatasetNames;
    }
}
