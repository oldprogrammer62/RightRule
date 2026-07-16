using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;
using RightRule.System.Common.Exceptions;

namespace RightRule;

public sealed class RightRule : IRightRule
{
    private readonly RightRuleService _rightRuleService;

    public bool SilentErrors => _rightRuleService.SilentErrors;

    public RightRule()
    {
        _rightRuleService = new RightRuleService();
    }

    public List<ParsedRule> GetRules()
    {
        return _rightRuleService.GetParsedRules();
    }

    public List<ParsedRule> GetRules(string Context)
    {
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        return _rightRuleService.GetRules(Context);
    }

    /// <summary>
    /// Registers a set of rules with the RightRule engine.
    /// </summary>
    /// <param name="rules">The rule definitions to register.</param>
    /// <returns>The parsed rules.</returns>
    /// <exception cref="SyntaxErrorException">Thrown when any of the rules contain a syntax error.</exception>
    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        try
        {
            return _rightRuleService.RegisterRules(rules);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a single rule with the RightRule engine.
    /// </summary>
    /// <param name="rule">The rule definition to register.</param>
    /// <returns>The parsed rule.</returns>
    /// <exception cref="SyntaxErrorException">Thrown when the rule contains a syntax error.</exception>
    public ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        try
        {
            return _rightRuleService.RegisterRule(rule);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregisters all rules associated with the specified context from the RightRule engine.
    /// </summary>
    /// <param name="Context">The context for which to deregister rules.</param>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        return _rightRuleService.DeRegisterRules(Context);
    }

    /// <summary>
    /// Deregisters all rules from the RightRule engine, regardless of context.
    /// </summary>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    public bool DeRegisterRules()
    {
        return _rightRuleService.DeRegisterRules();
    }

    public RegisteredDatasets GetRegisteredDatasets()
    {
        return _rightRuleService.GetDataSets();
    }

    public RegisteredDataset? GetRegisteredDataset(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrEmpty(DatasetName, nameof(DatasetName));
        return _rightRuleService.GetDataSet(DatasetName);
    }

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <typeparam name="T">The type of the data to register.</typeparam>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="Data">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the data is null.</exception>
    /// <exception cref="ConvertDataException">Thrown when the data cannot be converted to a dictionary format.</exception>
    public bool RegisterData<T>(string DatasetName, T Data)
    {
        ArgumentNullException.ThrowIfNull(Data);
        ArgumentException.ThrowIfNullOrEmpty(DatasetName, nameof(DatasetName));
        try
        {
            return _rightRuleService.RegisterData<T>(DatasetName, Data);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="Data">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the data is null.</exception>
    /// <exception cref="ConvertDataException">Thrown when the data cannot be converted to a dictionary format.</exception>
    public bool RegisterData(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data);
        ArgumentException.ThrowIfNullOrEmpty(DatasetName, nameof(DatasetName));
        try
        {
            return _rightRuleService.RegisterData(DatasetName, Data);
        }
        catch
        {
            throw;
        }
    }

    public void DeRegisterData(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrEmpty(DatasetName, nameof(DatasetName));
        _rightRuleService.DeRegisterData(DatasetName);
    }

    public bool UnloadOperator(string operatorName)
    {
        ArgumentException.ThrowIfNullOrEmpty(operatorName, nameof(operatorName));
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
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        return _rightRuleService.ApplyRule(Context);
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
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrEmpty(DatasetName, nameof(DatasetName));
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
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        return _rightRuleService.ApplyRules(Context);
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
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
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
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrEmpty(DatasetName, nameof(DatasetName));
        return _rightRuleService.ApplyRules(Context, DatasetName, Data);
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
        ArgumentException.ThrowIfNullOrEmpty(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrEmpty(DatasetName, nameof(DatasetName));
        return _rightRuleService.ApplyRules(Context, DatasetName, Data, rulesToReturn);
    }
}
