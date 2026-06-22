using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule;

internal class RightRuleService
{
    private readonly Dictionary<string, IOperator> _operators;

    private readonly ParsedRules _rules;
    private readonly RegisteredDatasets _registeredData;

    public RightRuleService()
    {
        var _operatorNames = GetOperatorNames();
        _operators = GetOperators(_operatorNames);
        _rules = new ParsedRules();
        _registeredData = new RegisteredDatasets();
    }

    internal List<ParsedRule> GetRules()
    {
        return _rules.RightRules;
    }

    internal List<ParsedRule> GetRules(string Context)
    {
        var rules = _rules.RightRules.Where(r => r.Context == Context).ToList();
        return rules;
    }

    internal List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var operatorNames = _operators.Select(o => o.Value.Identifiers);

        var parsedRules = RuleRegistrationService.ParseRules(rules, operatorNames);
        _rules.AddRules(parsedRules);
        return parsedRules;
    }

    internal ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var operatorNames = _operators.Select(o => o.Value.Identifiers);

        var parsedRule = RuleRegistrationService.ParseRule(rule, operatorNames);
        _rules.AddRule(parsedRule);
        return parsedRule;
    }

    internal bool DeRegisterRules(string Context)
    {
        ArgumentNullException.ThrowIfNull(Context);
        if (string.IsNullOrWhiteSpace(Context))
        {
            throw new ArgumentException("Context cannot be null or empty.", nameof(Context));
        }
        return _rules.RemoveRules(Context);
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
        if (_rules.RightRules == null || _rules.RightRules.Count == 0)
        {
            return AppliedRule.DefaultRule("Error: No rules available for application.");
        }
        return RuleApplicationService.ApplyRule(Context, _registeredData, _rules, _operators);
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
        if (_rules.RightRules == null || _rules.RightRules.Count == 0)
        {
            return AppliedRule.DefaultRule("Error: No rules available for application.");
        }
        return RuleApplicationService.ApplyRule(Context, _registeredData, _rules, _operators, DatasetName);
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
        if (_rules.RightRules == null || _rules.RightRules.Count == 0)
        {
            return [AppliedRule.DefaultRule("Error: No rules available for application.")];
        }
        return RuleApplicationService.ApplyRules(Context, _registeredData, _rules, rulesToReturn, _operators);
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
        if (_rules.RightRules == null || _rules.RightRules.Count == 0)
        {
            return [AppliedRule.DefaultRule("Error: No rules available for application.")];
        }
        return RuleApplicationService.ApplyRules(Context, _registeredData, _rules, rulesToReturn, _operators, DatasetName);
    }

    private List<AppliedRule> ApplySpecifiedRules<T>(string Context, string DatasetName, T Data, RulesToReturn rulesToReturn)
    {
        RegisterData(DatasetName, Data);
        List<AppliedRule> result = ApplySpecifiedRules(Context, rulesToReturn);
        DeRegisterData(DatasetName);
        return result;
    }

    private static List<IOperator> GetOperatorNames()
    {
        return Operators.Operators.RegisterOperatorNames();
    }

    private static Dictionary<string, IOperator> GetOperators(List<IOperator> operators)
    {
        var operatorNames = new Dictionary<string, IOperator>(StringComparer.OrdinalIgnoreCase);
        foreach (var op in operators)
        {
            foreach (var name in op.Identifiers)
            {
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                operatorNames[name] = op;
            }
        }
        return operatorNames;
    }
}
