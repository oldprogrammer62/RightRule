using RightRule.Data;
using RightRule.Rules;
using RightRule.Operators;
using RightRule.System.Common;
using RightRule.System.Common.Exceptions;
using RightRule.HelperFunctions;

namespace RightRule;

internal class RightRuleService
{
    private readonly OperatorsService _operatorsService;
    private readonly ParsedRuleService _rulesService;
    private readonly RegisteredDatasets _registeredDatasets;

    private bool _silentErrors = false;
    public bool SilentErrors => _silentErrors;

    public RightRuleService()
    {
        _operatorsService = new OperatorsService();
        _rulesService = new ParsedRuleService();
        _registeredDatasets = new RegisteredDatasets();

        _silentErrors = ConfigurationHelper.GetSetting("SilentEvaluatorExceptions")?.ToLower() == "true";
    }

    internal bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrEmpty(OperatorName, nameof(OperatorName));
        return _operatorsService.DeRegisterOperator(OperatorName);
    }

    internal List<ParsedRule> GetParsedRules()
    {
        return _rulesService.ParsedRules.Rules;
    }

    internal List<ParsedRule> GetRules(string Context)
    {
        return _rulesService.GetRules(Context);
    }

    /// <summary>
    /// Registers a collection of rules with the service, parsing them and making them available for application.
    /// </summary>
    /// <param name="rules">The rule definitions to register.</param>
    /// <returns>The parsed rules.</returns>
    /// <exception cref="SyntaxErrorException">Thrown when any of the rules contain a syntax error.</exception>
    internal List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var operatorNames = _operatorsService.OperatorNames;
        return _rulesService.RegisterRules(rules, operatorNames);
    }

    /// <summary>
    /// Registers a single rule with the service, parsing it and making it available for application.
    /// </summary>
    /// <param name="rule">The rule definition to register.</param>
    /// <returns>The parsed rule.</returns>
    /// <exception cref="SyntaxErrorException">Thrown when the rule contains a syntax error.</exception>
    internal ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var operatorNames = _operatorsService.OperatorNames;
        return _rulesService.RegisterRule(rule, operatorNames);
    }

    internal bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        return _rulesService.DeRegisterRules(Context);
    }

    internal bool DeRegisterRules()
    {
        return _rulesService.DeRegisterRules();
    }

    internal RegisteredDatasets GetDataSets()
    {
        return _registeredDatasets;
    }

    internal RegisteredDataset? GetDataSet(string DatasetName)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }

        return _registeredDatasets.GetDataset(DatasetName);
    }

    internal bool RegisterData(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        ArgumentNullException.ThrowIfNull(Data);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        var data = DataRegistrationService.CreateRegisteredDataObject(DatasetName, Data);
        return _registeredDatasets.AddDataset(data);
    }

    internal void DeRegisterData(string DatasetName)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        _registeredDatasets.RemoveDataset(DatasetName);
    }

    internal AppliedRule ApplyRule(string Context)
    {
        return ApplySingleRule(Context);
    }

    internal AppliedRule ApplyRule(string Context, string DatasetName, Dictionary<string, object> Data)
    {
        RegisterData(DatasetName, Data);
        AppliedRule result = ApplySingleRule(Context);
        DeRegisterData(DatasetName);
        return result;
    }

    private AppliedRule ApplySingleRule(string Context)
    {
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        if (_registeredDatasets == null || _registeredDatasets.Count == 0)
        {
            return AppliedRule.DefaultRule("Error: No registered data available for rule application.");
        }
        if (_rulesService.ParsedRules.Rules == null || _rulesService.ParsedRules.Rules.Count == 0)
        {
            return AppliedRule.DefaultRule("Error: No rules available for application.");
        }
        return RuleApplicationService.ApplyRule(Context, _registeredDatasets, _rulesService.ParsedRules, _operatorsService.OperatorAliases, _silentErrors);
    }

    internal List<AppliedRule> ApplyRules(string Context)
    {
        return ApplySpecifiedRules(Context, RulesToReturn.All);
    }

    internal List<AppliedRule> ApplyRules(string Context, RulesToReturn rulesToReturn)
    {
        return ApplySpecifiedRules(Context, rulesToReturn);
    }

    internal List<AppliedRule> ApplyRules(string Context, string DatasetName, Dictionary<string, object> Data)
    {
        return ApplySpecifiedRules(Context, DatasetName, Data, RulesToReturn.All);
    }

    internal List<AppliedRule> ApplyRules(string Context, string DatasetName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        return ApplySpecifiedRules(Context, DatasetName, Data, rulesToReturn);
    }

    private List<AppliedRule> ApplySpecifiedRules(string Context, string DatasetName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        RegisterData(DatasetName, Data);
        List<AppliedRule> result = ApplySpecifiedRules(Context, rulesToReturn);
        DeRegisterData(DatasetName);
        return result;
    }

    private List<AppliedRule> ApplySpecifiedRules(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        if (_registeredDatasets == null || _registeredDatasets.Count == 0)
        {
            return [AppliedRule.DefaultRule("Error: No registered data available for rule application.")];
        }
        if (_rulesService.ParsedRules.Rules == null || _rulesService.ParsedRules.Rules.Count == 0)
        {
            return [AppliedRule.DefaultRule("Error: No rules available for application.")];
        }
        return RuleApplicationService.ApplyRules(Context, _registeredDatasets, _rulesService.ParsedRules, rulesToReturn, _operatorsService.OperatorAliases);
    }
}
