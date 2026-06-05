using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule;

public class RightRule
{
    private readonly RightRuleService _service;

    public RightRule()
    {
        _service = new RightRuleService();
    }

    public List<ParsedRule> GetRules()
    {
        return _service.GetRules();
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _service.GetRules(Context);
    }

    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        return _service.RegisterRules(rules);
    }

    public ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        return _service.RegisterRule(rule);
    }

    public bool DeRegisterRules(string Context)
    {
        ArgumentNullException.ThrowIfNull(Context);
        if (string.IsNullOrWhiteSpace(Context))
        {
            throw new ArgumentException("Context cannot be null or empty.", nameof(Context));
        }
        return _service.DeRegisterRules(Context);
    }

    public RegisteredDatasets GetRegisteredDatasets()
    {
        return _service.GetDataSets();
    }

    public RegisteredDataset? GetRegisteredDataset(string DatasetName)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        return _service.GetDataSet(DatasetName);
    }

    public bool RegisterData<T>(string DatasetName, T Data)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        ArgumentNullException.ThrowIfNull(Data);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        return _service.RegisterData(DatasetName, Data);
    }

    public bool RegisterData(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        ArgumentNullException.ThrowIfNull(Data);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        return _service.RegisterData(DatasetName, Data);
    }

    public void DeRegisterData(string DatasetName)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        _service.DeRegisterData(DatasetName);
    }

    public AppliedRule ApplyRules(string Context)
    {
        return _service.ApplyRules(Context);
    }

    public AppliedRule ApplyRules<T>(string Context, string DatasetName, T Data)
    {
        return _service.ApplyRules(Context, DatasetName, Data);
    }

    public AppliedRule ApplyRules<T>(string Context, string DatasetName, T Data, RulesToApply RulesToApply)
    {
        return _service.ApplyRules(Context, DatasetName, Data, RulesToApply);
    }

    public AppliedRule ApplyRules(string Context, RulesToApply RulesToApply)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return AppliedRule.DefaultRule("Rule Context cannot be null or empty.");
        }
        return _service.ApplyRules(Context, RulesToApply);
    }
}
