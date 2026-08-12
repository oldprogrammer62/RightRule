using RightRule.Data;
using RightRule.Models;
using RightRule.Operators;
using RightRule.RuleApplication;
using RightRule.RuleRegistration;
using RightRule.RuleModels;
using RightRule.System.Common;
using RightRule.RuleConfiguration;

namespace BeSoft;

internal class RightRuleService
{
    private readonly OperatorsService _operatorsService;

    private readonly RegisteredRuleService _rulesService;
    internal RegisteredRules RegisteredRules => _rulesService.RegisteredRules;

    private readonly RegisteredDatasets? _registeredDataSets;
    internal RegisteredDatasets RegisteredDatasets => _registeredDataSets!;

    private readonly RegisteredDataObjects? _registeredDataObjects;
    internal RegisteredDataObjects RegisteredDataObjects => _registeredDataObjects!;

    private readonly RightRuleConfiguration _rightRuleConfig;
    internal RightRuleConfiguration RightRuleConfig => _rightRuleConfig;

    public RightRuleService()
    {
        _operatorsService = new OperatorsService();
        _rulesService = new RegisteredRuleService();
        _registeredDataSets = new RegisteredDatasets();
        _registeredDataObjects = new RegisteredDataObjects();

        _rightRuleConfig = GetConfiguration();
    }

    private static RightRuleConfiguration GetConfiguration()
    {
        return ConfigurationManager.GetConfiguration();
    }

    internal Dictionary<string, Operator> GetOperatorAliases()
    {
        return _operatorsService.OperatorAliases;
    }

    internal List<Operator> GetOperatorDetails()
    {
        return _operatorsService.Operators;
    }

    internal bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        return _operatorsService.DeRegisterOperator(OperatorName);
    }

    internal ParsedRule? GetRegisteredRule(string Name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name, nameof(Name));
        return _rulesService.GetRegisteredRule(Name);
    }

    internal ParsedRule? GetRegisteredRule(int Number)
    {
        return _rulesService.GetRegisteredRule(Number);
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
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException">Thrown when any of the rules contain a syntax error.</exception>
    internal List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));

        var operatorNames = _operatorsService.OperatorNames;
        return _rulesService.RegisterRules(rules, operatorNames);
    }

    /// <summary>
    /// Registers a single rule with the service, parsing it and making it available for application.
    /// </summary>
    /// <param name="rule">The rule definition to register.</param>
    /// <returns>The parsed rule.</returns>
    /// <exception cref="FormatException">Thrown when the rule contains a syntax error.</exception>
    internal ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));

        var operatorNames = _operatorsService.OperatorNames;
        return _rulesService.RegisterRule(rule, operatorNames);
    }

    internal bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return _rulesService.DeRegisterRules(Context);
    }

    internal bool DeRegisterRules()
    {
        return _rulesService.DeRegisterRules();
    }

    internal bool DeRegisterRule(string Name)
    {
        return _rulesService.DeRegisterRule(Name);
    }

    internal bool DeRegisterRule(int Number)
    {
        return _rulesService.DeRegisterRule(Number);
    }

    internal RegisteredDataset? GetDataSet(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        if (_registeredDataSets == null)
        {
            return null;
        }
        return _registeredDataSets.GetData(DataName);
    }

    internal RegisteredDataObject? GetDataObject(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        if (_registeredDataObjects == null)
        {
            return null;
        }
        return _registeredDataObjects.GetData(DataName);
    }

    internal bool RegisterData(string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        var data = DataRegistrationService.CreateRegisteredData(DataName, Data);
        return _registeredDataSets!.AddData(data);
    }

    internal bool RegisterData(string DataName, object DataReference)
    {
        ArgumentNullException.ThrowIfNull(DataReference, nameof(DataReference));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        var data = DataRegistrationService.CreateRegisteredData(DataName, DataReference);
        return _registeredDataObjects!.AddData(data);
    }

    internal bool DeRegisterData()
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

    internal bool DeRegisterData(string DataName)
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

    internal async Task<List<AppliedRule>> EvaluateFirstRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSingleRuleAsync(Context).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> EvaluateFirstRuleAsync(string Context, string DataName, Dictionary<string, object> Data)
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

        if (_rulesService.RegisteredRules.Rules == null || _rulesService.RegisteredRules.Rules.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRuleAsync(Context, _registeredDataSets, _registeredDataObjects, _rulesService.RegisteredRules, _operatorsService.OperatorAliases, this.RightRuleConfig).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSpecifiedRulesAsync(Context, RulesToReturn.All).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSpecifiedRulesAsync(Context, ReturnRules).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        return await EvaluateSpecifiedRulesAsync(Context, DataName, Data, RulesToReturn.All).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
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

        if (_rulesService.RegisteredRules.Rules == null || _rulesService.RegisteredRules.Rules.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRulesAsync(Context, _registeredDataSets, _registeredDataObjects, _rulesService.RegisteredRules, ReturnRules, _operatorsService.OperatorAliases, this.RightRuleConfig).ConfigureAwait(false);
    }
}
