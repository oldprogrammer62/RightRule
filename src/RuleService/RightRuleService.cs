using RightRule.Data;
using RightRule.Operators;
using RightRule.RuleApplication;
using RightRule.RuleRegistration;
using RightRule.RuleModels;
using RightRule.System.Common;
using BeSoft;
using RightRule.RuleLoaders;
// using RightRule.RuleConfiguration;

namespace RightRule.RuleService;

internal class RightRuleService : IRightRuleService
{
    private readonly IOperatorsService _operatorsService;

    private readonly IRegisteredRuleService _registerRuleService;
    public IRegisteredRules RegisteredRules => _registerRuleService.RegisteredRules;

    private readonly IRegisteredDatasets? _registeredDataSets;
    public IRegisteredDatasets RegisteredDatasets => _registeredDataSets!;

    private readonly IRegisteredDataObjects? _registeredDataObjects;
    public IRegisteredDataObjects RegisteredDataObjects => _registeredDataObjects!;

    public RightRuleService(IOperatorsService operatorsService, IRegisteredRuleService registeredRuleService, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects)
    {
        _operatorsService = operatorsService;
        _registerRuleService = registeredRuleService;
        _registeredDataSets = registeredDatasets;
        _registeredDataObjects = registeredDataObjects;
    }

    public Dictionary<string, Operator> GetOperatorAliases()
    {
        return _operatorsService.OperatorAliases;
    }

    public List<Operator> GetOperatorDetails()
    {
        return _operatorsService.Operators;
    }

    public bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        return _operatorsService.DeRegisterOperator(OperatorName);
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

    public List<ParsedRule>? RegisterRulesJson(IRightRule rightRule, string Data)
    {
        var ruleDefn = RuleLoaderService.LoadRulesFromJson(rightRule, Data);
        if (ruleDefn == null)
        {
            return default(List<ParsedRule>);
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
    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
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
    public ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));

        var operatorNames = _operatorsService.OperatorNames;

        var objectNames = GetObjectNames();
        var datasetNames = GetDatasetNames();

        return _registerRuleService.RegisterRule(rule, operatorNames, objectNames, datasetNames);
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

    public RegisteredDataset? GetDataSet(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        if (_registeredDataSets == null)
        {
            return null;
        }
        return _registeredDataSets.GetData(DataName);
    }

    public RegisteredDataObject? GetDataObject(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        if (_registeredDataObjects == null)
        {
            return null;
        }
        return _registeredDataObjects.GetData(DataName);
    }

    public bool RegisterData(string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        var data = DataRegistrationService.CreateRegisteredData(DataName, Data);
        return _registeredDataSets!.AddData(data);
    }

    public bool RegisterData(string DataName, object VariableValue)
    {
        ArgumentNullException.ThrowIfNull(VariableValue, nameof(VariableValue));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        var data = DataRegistrationService.CreateRegisteredData(DataName, VariableValue);
        return _registeredDataObjects!.AddData(data);
    }

    public bool DeRegisterData()
    {
        DeregisterDataObjects();
        DergisterDatasets();
        return true;
    }

    private void DeregisterDataObjects()
    {
        if (_registeredDataObjects == null)
        {
            return;
        }
        _registeredDataObjects.RemoveData();
    }

    private void DergisterDatasets()
    {
        if (_registeredDataSets == null)
        {
            return;
        }
        _registeredDataSets.RemoveData();
    }

    public bool DeRegisterData(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        DeRegisterDataObjects(DataName);
        DeRegisterDatasets(DataName);
        return true;
    }

    private void DeRegisterDataObjects(string DataName)
    {
        if (_registeredDataObjects == null)
        {
            return;
        }
        _registeredDataObjects.RemoveData(DataName);
    }

    private void DeRegisterDatasets(string DataName)
    {
        if (_registeredDataSets == null)
        {
            return;
        }
        _registeredDataSets.RemoveData(DataName);
    }

    public async Task<List<AppliedRule>> EvaluateFirstRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSingleRuleAsync(Context).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateFirstRuleAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        RegisterData(DataName, Data);
        List<AppliedRule> appliedRules = await EvaluateSingleRuleAsync(Context).ConfigureAwait(false);
        DeRegisterData(DataName);
        return appliedRules;
    }

    private async Task<List<AppliedRule>> EvaluateSingleRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registerRuleService.RegisteredRules.Rules == null || _registerRuleService.RegisteredRules.Rules.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRuleAsync(Context, _registeredDataSets, _registeredDataObjects, _registerRuleService.RegisteredRules, _operatorsService.OperatorAliases).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSpecifiedRulesAsync(Context, RulesToReturn.All).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSpecifiedRulesAsync(Context, ReturnRules).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        return await EvaluateSpecifiedRulesAsync(Context, DataName, Data, RulesToReturn.All).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        return await EvaluateSpecifiedRulesAsync(Context, DataName, Data, ReturnRules).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> EvaluateSpecifiedRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        RegisterData(DataName, Data);
        List<AppliedRule> appliedRules = await EvaluateSpecifiedRulesAsync(Context, ReturnRules).ConfigureAwait(false);
        DeRegisterData(DataName);
        return appliedRules;
    }

    private async Task<List<AppliedRule>> EvaluateSpecifiedRulesAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registerRuleService.RegisteredRules.Rules == null || _registerRuleService.RegisteredRules.Rules.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRulesAsync(Context, _registeredDataSets, _registeredDataObjects, _registerRuleService.RegisteredRules, ReturnRules, _operatorsService.OperatorAliases).ConfigureAwait(false);
    }
}
