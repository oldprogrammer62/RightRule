using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule;

public sealed class RightRule
{
    private readonly RightRuleService _rightRuleService;

    public RightRule()
    {
        _rightRuleService = new RightRuleService();
    }

    public List<ParsedRule> GetRules()
    {
        return _rightRuleService.GetRules();
    }

    public List<ParsedRule> GetRules(string Context)
    {
        return _rightRuleService.GetRules(Context);
    }

    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        return _rightRuleService.RegisterRules(rules);
    }

    public ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        return _rightRuleService.RegisterRule(rule);
    }

    public bool DeRegisterRules(string Context)
    {
        ArgumentNullException.ThrowIfNull(Context);
        if (string.IsNullOrWhiteSpace(Context))
        {
            throw new ArgumentException("Context cannot be null or empty.", nameof(Context));
        }
        return _rightRuleService.DeRegisterRules(Context);
    }

    public RegisteredDatasets GetRegisteredDatasets()
    {
        return _rightRuleService.GetDataSets();
    }

    public RegisteredDataset? GetRegisteredDataset(string DatasetName)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        return _rightRuleService.GetDataSet(DatasetName);
    }

    public bool RegisterData<T>(string DatasetName, T Data)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        ArgumentNullException.ThrowIfNull(Data);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        return _rightRuleService.RegisterData(DatasetName, Data);
    }

    public bool RegisterData(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        ArgumentNullException.ThrowIfNull(Data);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        return _rightRuleService.RegisterData(DatasetName, Data);
    }

    public void DeRegisterData(string DatasetName)
    {
        ArgumentNullException.ThrowIfNull(DatasetName);
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(DatasetName));
        }
        _rightRuleService.DeRegisterData(DatasetName);
    }

    public AppliedRule ApplyRule(string Context)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return AppliedRule.DefaultRule("Rule Context cannot be null or empty.");
        }
        return _rightRuleService.ApplyRule(Context);
    }

    public AppliedRule ApplyRule<T>(string Context, string DatasetName, T Data)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return AppliedRule.DefaultRule("Rule Context cannot be null or empty.");
        }
        return _rightRuleService.ApplyRule<T>(Context, DatasetName, Data);
    }

    public List<AppliedRule> ApplyRules(string Context)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context);
    }

    public List<AppliedRule> ApplyRules(string Context, RulesToApply RulesToApply)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context, RulesToApply);
    }

    public List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, T Data)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context, DatasetName, Data);
    }

    public List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, T Data, RulesToApply RulesToApply)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context, DatasetName, Data, RulesToApply);
    }
}
