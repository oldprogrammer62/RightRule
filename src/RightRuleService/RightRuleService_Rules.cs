using RightRule.RuleModels;
using BeSoft;
using RightRule.RuleLoaders;
using RightRule.RuleRegistration.LogicRules;
using RightRule.RuleModels.BaseModels;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    public RuleDefault GetDefaultRule()
    {
        return _defaultRuleRegister.GetDefaultRule();
    }

    public List<(int Number, string Name)>? GetRuleNumbers()
    {
        return _registeredRuleService.GetRuleNumbers();
    }

    public List<string>? GetRuleNames()
    {
        return _registeredRuleService.GetRuleNames();
    }

    public List<string>? GetRuleContexts()
    {
        return _registeredRuleService.GetRuleContexts();
    }

    public ParsedRule? GetRegisteredRule(string Name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name, nameof(Name));
        return _registeredRuleService.GetRegisteredRule(Name);
    }

    public ParsedRule? GetRegisteredRule(int Number)
    {
        return _registeredRuleService.GetRegisteredRule(Number);
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _registeredRuleService.GetRules(Context);
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
        return _registeredRuleService.RegisterRules(rules, operatorNames, objectNames, datasetNames);
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

        return _registeredRuleService.RegisterRule(rule, operatorNames, objectNames, datasetNames);
    }

    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return _registeredRuleService.DeRegisterRules(Context);
    }

    public bool DeRegisterRules()
    {
        return _registeredRuleService.DeRegisterRules();
    }

    public bool DeRegisterRule(string Name)
    {
        return _registeredRuleService.DeRegisterRule(Name);
    }

    public bool DeRegisterRule(int Number)
    {
        return _registeredRuleService.DeRegisterRule(Number);
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
