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

    public bool UnloadOperator(string operatorName)
    {
        ArgumentNullException.ThrowIfNull(operatorName);
        if (string.IsNullOrWhiteSpace(operatorName))
        {
            throw new ArgumentException("Operator name cannot be null or empty.", nameof(operatorName));
        }
        return _rightRuleService.UnloadOperator(operatorName);
    }

    /// <summary>
    /// Apply all registered rules for the given context to the registered data set(s).
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    public AppliedRule ApplyRule(string Context)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return AppliedRule.DefaultRule("Rule Context cannot be null or empty.");
        }
        return _rightRuleService.ApplyRule(Context);
    }

    /// <summary>
    /// Apply all registered rules for the given context to the specified data set.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DatasetName">The name of the registered dataset to use</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    public AppliedRule ApplyRule(string Context, string DatasetName)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return AppliedRule.DefaultRule("Rule Context cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(DatasetName))
        {
            return AppliedRule.DefaultRule("Dataset name cannot be null or empty.");
        }
        return _rightRuleService.ApplyRule(Context, DatasetName);
    }

    /// <summary>
    /// Apply all registered rules for the given context to the data set provided.
    /// </summary>
    /// <typeparam name="T">Valid types are: Dictionary, Enumerable, object with name/value pairs</typeparam>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DatasetName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    public AppliedRule ApplyRule<T>(string Context, string DatasetName, T Data)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return AppliedRule.DefaultRule("Rule Context cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(DatasetName))
        {
            return AppliedRule.DefaultRule("Dataset name cannot be null or empty.");
        }
        return _rightRuleService.ApplyRule<T>(Context, DatasetName, Data);
    }

    /// <summary>
    /// Apply all registered rules for the given context to the registered data set(s).
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// All Rules applied are returned in a list.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    public List<AppliedRule> ApplyRules(string Context)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context);
    }

    /// <summary>
    /// Apply all registered rules for the given context to the specified data set.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DatasetName">The name of the registered dataset to use</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// All Rules applied are returned in a list.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    public List<AppliedRule> ApplyRules(string Context, string DatasetName)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        if (string.IsNullOrEmpty(DatasetName))
        {
            return [AppliedRule.DefaultRule("Dataset name cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context, DatasetName);
    }

    /// <summary>
    /// Apply specified registered rules (First, All) for the given context to the registered data set(s).
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="rulesToReturn">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified rulesToReturn parameter.
    /// </returns>
    public List<AppliedRule> ApplyRules(string Context, RulesToReturn rulesToReturn)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context, rulesToReturn);
    }

    /// <summary>
    /// Apply all registered rules for the given context to the specified data set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DatasetName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    public List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, T Data)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context, DatasetName, Data);
    }

    /// <summary>
    /// Apply all registered rules for the given context to the specified data set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DatasetName">The name of the registered dataset to use</param>
    /// <param name="rulesToReturn">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified rulesToReturn parameter.
    /// </returns>
    public List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, RulesToReturn rulesToReturn)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context, DatasetName, rulesToReturn);
    }

    /// <summary>
    /// Apply all registered rules for the given context to the specified data set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DatasetName">The name of the registered dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <param name="rulesToReturn">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified rulesToReturn parameter.
    /// </returns>
    public List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, T Data, RulesToReturn rulesToReturn)
    {
        if (string.IsNullOrEmpty(Context))
        {
            return [AppliedRule.DefaultRule("Rule Context cannot be null or empty.")];
        }
        return _rightRuleService.ApplyRules(Context, DatasetName, Data, rulesToReturn);
    }
}
