using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule;

internal class RightRuleService
{
    private readonly List<IOperator> _operators;
    private readonly Dictionary<string, IOperator> _operatorAliases;

    private readonly ParsedRules _rules;
    private readonly RegisteredDatasets _registeredData;

    public RightRuleService()
    {
        _operators = RegisterOperators();
        _operatorAliases = GetOperatorAliases(_operators);
        _rules = new ParsedRules();
        _registeredData = new RegisteredDatasets();
    }

    internal bool UnloadOperator(string operatorName)
    {
        ArgumentNullException.ThrowIfNull(operatorName);
        if (string.IsNullOrWhiteSpace(operatorName))
        {
            throw new ArgumentException("Operator name cannot be null or empty.", nameof(operatorName));
        }
        return DeregisterOperator(operatorName);
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

        var operatorNames = _operatorAliases.Select(o => o.Value.Identifiers);

        var parsedRules = RuleRegistrationService.ParseRules(rules, operatorNames);
        _rules.AddRules(parsedRules);
        return parsedRules;
    }

    internal ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var operatorNames = _operatorAliases.Select(o => o.Value.Identifiers);

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
        return RuleApplicationService.ApplyRule(Context, _registeredData, _rules, _operatorAliases);
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
        return RuleApplicationService.ApplyRule(Context, _registeredData, _rules, _operatorAliases, DatasetName);
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
        return RuleApplicationService.ApplyRules(Context, _registeredData, _rules, rulesToReturn, _operatorAliases);
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
        return RuleApplicationService.ApplyRules(Context, _registeredData, _rules, rulesToReturn, _operatorAliases, DatasetName);
    }

    private List<AppliedRule> ApplySpecifiedRules<T>(string Context, string DatasetName, T Data, RulesToReturn rulesToReturn)
    {
        RegisterData(DatasetName, Data);
        List<AppliedRule> result = ApplySpecifiedRules(Context, rulesToReturn);
        DeRegisterData(DatasetName);
        return result;
    }

    private static List<IOperator> RegisterOperators()
    {
        return Operators.Operators.RegisterOperators();
    }

    private bool DeregisterOperator(string operatorName)
    {
        var opToRemove = _operators.FirstOrDefault(op => op.Identifiers.Contains(operatorName, StringComparer.OrdinalIgnoreCase));
        if (opToRemove != null)
        {
            Operators.Operators.UnloadOperator(opToRemove);
            RemoveOperatorAliases(opToRemove);
            _operators.Remove(opToRemove);
            return true;
        }
        return false;
    }

    private void RemoveOperatorAliases(IOperator op)
    {
        foreach(var opp in _operatorAliases)
        {
            if (opp.Value.Equals(op))
            {
                _operatorAliases.Remove(opp.Key);
            }
        }
    }

    private static Dictionary<string, IOperator> GetOperatorAliases(List<IOperator> operators)
    {
        var operatorAliases = new Dictionary<string, IOperator>(StringComparer.OrdinalIgnoreCase);
        foreach (var op in operators)
        {
            foreach (var alias in op.Identifiers)
            {
                if (string.IsNullOrWhiteSpace(alias))
                    continue;
                operatorAliases[alias] = op;
            }
        }
        return operatorAliases;
    }
}
