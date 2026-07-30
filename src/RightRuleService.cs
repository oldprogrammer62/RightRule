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

    private readonly RegisteredDatasets? _registeredDatasets;
    internal RegisteredDatasets RegisteredDatasets => _registeredDatasets!;

    private readonly RightRuleConfiguration _rightRuleConfig;
    internal RightRuleConfiguration RightRuleConfig => _rightRuleConfig;

    public RightRuleService()
    {
        _operatorsService = new OperatorsService();
        _rulesService = new RegisteredRuleService();
        _registeredDatasets = new RegisteredDatasets();

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

    internal RegisteredDataset? GetDataSet(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        if (_registeredDatasets == null)
        {
            return null;
        }
        return _registeredDatasets.GetDataset(DatasetName);
    }

    internal bool RegisterDataset(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        var data = DataRegistrationService.CreateRegisteredDataObject(DatasetName, Data);
        return _registeredDatasets!.AddDataset(data);
    }

    internal void DeRegisterDatasets()
    {
        if (_registeredDatasets == null)
        {
            return;
        }
        _registeredDatasets.RemoveDatasets();
    }

    internal void DeRegisterDataset(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        if (_registeredDatasets == null)
        {
            return;
        }
        _registeredDatasets.RemoveDataset(DatasetName);
    }

    internal async Task<List<AppliedRule>> ApplyRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await ApplySingleRuleAsync(Context).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ApplyRuleAsync(string Context, string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        RegisterDataset(DatasetName, Data);
        List<AppliedRule> appliedRules = await ApplySingleRuleAsync(Context).ConfigureAwait(false);
        DeRegisterDataset(DatasetName);
        return appliedRules;
    }

    private async Task<List<AppliedRule>> ApplySingleRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredDatasets == null || _registeredDatasets.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No registered data available for rule application.")];
        }
        if (_rulesService.RegisteredRules.Rules == null || _rulesService.RegisteredRules.Rules.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRuleAsync(Context, _registeredDatasets, _rulesService.RegisteredRules, _operatorsService.OperatorAliases, this.RightRuleConfig).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ApplyRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await ApplySpecifiedRulesAsync(Context, RulesToReturn.All).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ApplyRulesAsync(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await ApplySpecifiedRulesAsync(Context, rulesToReturn).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ApplyRulesAsync(string Context, string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        return await ApplySpecifiedRulesAsync(Context, DatasetName, Data, RulesToReturn.All).ConfigureAwait(false);
    }

    internal async Task<List<AppliedRule>> ApplyRulesAsync(string Context, string DatasetName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        return await ApplySpecifiedRulesAsync(Context, DatasetName, Data, rulesToReturn).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> ApplySpecifiedRulesAsync(string Context, string DatasetName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        RegisterDataset(DatasetName, Data);
        List<AppliedRule> appliedRules = await ApplySpecifiedRulesAsync(Context, rulesToReturn).ConfigureAwait(false);
        DeRegisterDataset(DatasetName);
        return appliedRules;
    }

    private async Task<List<AppliedRule>> ApplySpecifiedRulesAsync(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredDatasets == null || _registeredDatasets.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No registered data available for rule application.")];
        }
        if (_rulesService.RegisteredRules.Rules == null || _rulesService.RegisteredRules.Rules.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRulesAsync(Context, _registeredDatasets, _rulesService.RegisteredRules, rulesToReturn, _operatorsService.OperatorAliases, this.RightRuleConfig).ConfigureAwait(false);
    }
}
