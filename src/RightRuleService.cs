using RightRule.Data;
using RightRule.Rules;
using RightRule.Operators;
using RightRule.System.Common;

namespace RightRule;

internal class RightRuleService
{
    private readonly OperatorsService _operatorsService;
    private readonly ParsedRuleService _rulesService;

    private readonly RegisteredDatasets _registeredData;

    public RightRuleService()
    {
        _operatorsService = new OperatorsService();
        _rulesService = new ParsedRuleService();

        _registeredData = new RegisteredDatasets();
    }

    internal bool UnloadOperator(string OperatorName)
    {
        ArgumentNullException.ThrowIfNull(OperatorName);
        if (string.IsNullOrWhiteSpace(OperatorName))
        {
            throw new ArgumentException("Operator name cannot be null or empty.", nameof(OperatorName));
        }
        return _operatorsService.DeRegisterOperator(OperatorName);
    }

    internal List<ParsedRule> GetParsedRules()
    {
        return _rulesService.ParsedRules.Rules;
    }

    internal List<ParsedRule> GetRules(string Context)
    {
        return _rulesService.GetRules(Context);
        //var rules = _rules.Rules.Where(r => r.Context == Context).ToList();
        //return rules;
    }

    internal List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var operatorNames = _operatorsService.OperatorNames;
        return _rulesService.RegisterRules(rules, operatorNames);
    }

    internal ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var operatorNames = _operatorsService.OperatorNames;
        return _rulesService.RegisterRule(rule, operatorNames);
    }

    internal bool DeRegisterRules(string Context)
    {
        ArgumentNullException.ThrowIfNull(Context);
        if (string.IsNullOrWhiteSpace(Context))
        {
            throw new ArgumentException("Context cannot be null or empty.", nameof(Context));
        }

        return _rulesService.DeRegisterRules(Context);
    }

    internal RegisteredDatasets GetDataSets()
    {
        return _registeredData;
    }

    internal RegisteredDataset? GetDataSet(string DatasetName)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }

        return _registeredData.GetDataset(DatasetName);
    }

    internal bool RegisterData<T>(string DatasetName, T Data)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        ArgumentNullException.ThrowIfNull(Data);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }

        var data = DataRegistrationService.CreateRegisteredDataObject<T>(DatasetName, Data);
        return _registeredData.AddDataset(data);
    }

    internal bool RegisterData(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        ArgumentNullException.ThrowIfNull(Data);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }

        var data = DataRegistrationService.CreateRegisteredDataObject<Dictionary<string, object>>(DatasetName, Data);
        return _registeredData.AddDataset(data);
    }

    internal void DeRegisterData(string DatasetName)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }

        _registeredData.RemoveDataset(DatasetName);
    }

    internal AppliedRule ApplyRule(string Context)
    {
        return ApplySingleRule(Context);
    }

    internal AppliedRule ApplyRule(string Context, string DatasetName)
    {
        return ApplySingleRule(Context, DatasetName);
    }

    internal AppliedRule ApplyRule<T>(string Context, string DatasetName, T Data)
    {
        RegisterData<T>(DatasetName, Data);
        AppliedRule result = ApplySingleRule(Context);
        DeRegisterData(DatasetName);
        return result;
    }

    private AppliedRule ApplySingleRule(string Context)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return AppliedRule.DefaultRule("Error: Context cannot be null or empty.");
        }
        if (_registeredData == null || _registeredData.Count == 0)
        {
            return AppliedRule.DefaultRule("Error: No registered data available for rule application.");
        }
        if (_rulesService.ParsedRules.Rules == null || _rulesService.ParsedRules.Rules.Count == 0)
        {
            return AppliedRule.DefaultRule("Error: No rules available for application.");
        }
        return RuleApplicationService.ApplyRule(Context, _registeredData, _rulesService.ParsedRules, _operatorsService.OperatorAliases);
    }

    private AppliedRule ApplySingleRule(string Context, string DatasetName)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return AppliedRule.DefaultRule("Error: Context cannot be null or empty.");
        }
        if (_registeredData == null || _registeredData.Count == 0)
        {
            return AppliedRule.DefaultRule("Error: No registered data available for rule application.");
        }
        if (_rulesService.ParsedRules.Rules == null || _rulesService.ParsedRules.Rules.Count == 0)
        {
            return AppliedRule.DefaultRule("Error: No rules available for application.");
        }
        return RuleApplicationService.ApplyRule(Context, _registeredData, _rulesService.ParsedRules, _operatorsService.OperatorAliases, DatasetName);
    }

    internal List<AppliedRule> ApplyRules(string Context)
    {
        return ApplySpecifiedRules(Context, RulesToReturn.All);
    }

    internal List<AppliedRule> ApplyRules(string Context, string DatasetName)
    {
        return ApplySpecifiedRules(Context, DatasetName, RulesToReturn.All);
    }

    internal List<AppliedRule> ApplyRules(string Context, RulesToReturn rulesToReturn)
    {
        return ApplySpecifiedRules(Context, rulesToReturn);
    }

    internal List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, T Data)
    {
        return ApplySpecifiedRules(Context, DatasetName, Data, RulesToReturn.All);
    }

    internal List<AppliedRule> ApplyRules(string Context, string DatasetName, RulesToReturn rulesToReturn)
    {
        return ApplySpecifiedRules(Context, DatasetName, rulesToReturn);
    }

    internal List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, T Data, RulesToReturn rulesToReturn)
    {
        return ApplySpecifiedRules(Context, DatasetName, Data, rulesToReturn);
    }

    private List<AppliedRule> ApplySpecifiedRules<T>(string Context, string DatasetName, T Data, RulesToReturn rulesToReturn)
    {
        RegisterData(DatasetName, Data);
        List<AppliedRule> result = ApplySpecifiedRules(Context, rulesToReturn);
        DeRegisterData(DatasetName);
        return result;
    }

    private List<AppliedRule> ApplySpecifiedRules(string Context, RulesToReturn rulesToReturn)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Error: Context cannot be null or empty.")];
        }
        if (_registeredData == null || _registeredData.Count == 0)
        {
            return [AppliedRule.DefaultRule("Error: No registered data available for rule application.")];
        }
        if (_rulesService.ParsedRules.Rules == null || _rulesService.ParsedRules.Rules.Count == 0)
        {
            return [AppliedRule.DefaultRule("Error: No rules available for application.")];
        }
        return RuleApplicationService.ApplyRules(Context, _registeredData, _rulesService.ParsedRules, rulesToReturn, _operatorsService.OperatorAliases);
    }

    private List<AppliedRule> ApplySpecifiedRules(string Context, string DatasetName, RulesToReturn rulesToReturn)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Error: Context cannot be null or empty.")];
        }
        if (_registeredData == null || _registeredData.Count == 0)
        {
            return [AppliedRule.DefaultRule("Error: No registered data available for rule application.")];
        }
        if (_rulesService.ParsedRules.Rules == null || _rulesService.ParsedRules.Rules.Count == 0)
        {
            return [AppliedRule.DefaultRule("Error: No rules available for application.")];
        }
        return RuleApplicationService.ApplyRules(Context, _registeredData, _rulesService.ParsedRules, rulesToReturn, _operatorsService.OperatorAliases, DatasetName);
    }
}