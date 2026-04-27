using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule;

internal class RightRuleService
{
    private readonly List<IOperator> _operators;
    private readonly ParsedRules _rules;
    private readonly RegisteredDatasets _registeredData;

    public RightRuleService()
    {
        _operators = GetOperators();
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

        var operatorNames = _operators.Select(o => o.Identifiers);
        var parsedRules = RuleRegistrationService.ParseRules(rules, operatorNames);
        _rules.AddRules(parsedRules);
        return parsedRules;
    }

    internal ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var operatorNames = _operators.Select(o => o.Identifiers);
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
        var data = DataRegistrationService.CreateRegisteredDataObject(DatasetName, Data);
        _registeredData.AddDataset(data);
        return true;
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
        _registeredData.AddDataset(data);
        return true;
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

    internal AppliedRule ApplyRules(string Context)
    {
        return ApplyRules(Context, RulesToApply.First);
    }

    internal AppliedRule ApplyRules<T>(string Context, string DatasetName, T Data)
    {
        return ApplyRules(Context, DatasetName, Data, RulesToApply.First);
    }

    internal AppliedRule ApplyRules<T>(string Context, string DatasetName, T Data, RulesToApply RulesToApply)
    {
        RegisterData(DatasetName, Data);
        AppliedRule result = ApplyRules(Context, RulesToApply);
        DeRegisterData(DatasetName);
        return result;
    }

    internal AppliedRule ApplyRules(string Context, RulesToApply RulesToApply)
    {
        if (string.IsNullOrEmpty(Context))
        {
            throw new ArgumentException("Context cannot be null or empty.", nameof(Context));
        }
        if (_registeredData == null || _registeredData.Count == 0)
        {
            throw new ArgumentNullException(nameof(_registeredData), "RegisteredData cannot be null or empty.");
        }
        if (_rules.RightRules == null || _rules.RightRules.Count == 0)
        {
            throw new ArgumentNullException(nameof(RulesToApply), "RulesToApply cannot be null or empty.");
        }

        var rules = Helpers.FilterRules(Context, _rules.RightRules);
        return RuleApplicationService.ApplyRules(Context, _registeredData, rules, RulesToApply, _operators);
    }

    private static List<IOperator> GetOperators()
    {
        return Operators.Operators.RegisterOperators();
    }
}
