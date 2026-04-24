using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule;

public class RightRule
{
    private readonly List<IOperator> _operators;
    private readonly ParsedRules _rules;
    private readonly RegisteredDatasets _registeredData;

    public RightRule()
    {
        _operators = GetOperators();
        _rules = new ParsedRules();
        _registeredData = new RegisteredDatasets();
    }

    /// <summary>
    /// Takes a set of Rule Definitions and registers them for use.
    /// </summary>
    /// <remarks>
    /// The process of registering rule includes parsing the rule statement
    /// and associating it with the available operators.
    /// </remarks>
    /// <param name="rules"></param>
    /// <returns>A List of ParsedRule objects</returns>
    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        var operatorNames = _operators.Select(o => o.Identifiers);
        var parsedRules = RuleRegistrationService.ParseRules(rules, operatorNames);
        _rules.AddRules(parsedRules);
        return parsedRules;
    }

    /// <summary>
    /// Takes a Rule Definition and registers it for use.
    /// </summary>
    /// <remarks>
    /// The process of registering rule includes parsing the rule statement
    /// and associating it with the available operators.
    /// </remarks>
    /// <param name="rule"></param>
    /// <returns>A ParsedRule object</returns>
    public ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        var operatorNames = _operators.Select(o => o.Identifiers);
        var parsedRule = RuleRegistrationService.ParseRule(rule, operatorNames);
        _rules.AddRule(parsedRule);
        return parsedRule;
    }

    /// <summary>
    /// Remove registered rules based on the provided context. All rules associated with the specified context will be removed from the system.
    /// </summary>
    /// <param name="Context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public bool RemoveRules(string Context)
    {
        ArgumentNullException.ThrowIfNull(Context);
        if (string.IsNullOrWhiteSpace(Context))
        {
            throw new ArgumentException("Context cannot be null or empty.", nameof(Context));
        }
        return _rules.RemoveRules(Context);
    }

    /// <summary>
    /// Registers Data for use in rule evaluation.
    /// </summary>
    /// <typeparam name="T">The type of the data being registered.</typeparam>
    /// <param name="DatasetName">The name of the dataset being registered. Cannot be null or empty.</param>
    /// <param name="Data">The data to be registered. Can be of any type that can be converted to a dictionary.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    public bool RegisterData<T>(string DatasetName, T Data) 
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

    /// <summary>
    /// Registers Data for use in rule evaluation.
    /// </summary>
    /// <param name="Data">The data to be registered.</param>
    /// <param name="DatasetName">The name of the dataset being registered. Cannot be null or empty.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    public bool RegisterData(string DatasetName, Dictionary<string, object> Data)
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

    /// <summary>
    /// Applies the default set of rules to the specified context and data, and returns the result of the first matching
    /// rule.
    /// </summary>
    /// <param name="Context">The context identifier used to determine which rules to apply. Cannot be null or empty.</param>
    /// <returns>An AppliedRule object representing the result of the first rule that matches the provided context and data.</returns>
    public AppliedRule ApplyRules(string Context)
    {
        return ApplyRules(Context, RulesToApply.First);
    }

    /// <summary>
    /// Applies the specified set of rules to the provided data within the given context and returns the result of the
    /// rule evaluation.
    /// </summary>
    /// <param name="Context">The context identifier used to filter and select applicable rules. Cannot be null or empty.</param>
    /// <param name="RulesToApply">Specifies which rules to apply during evaluation. Determines the subset or type of rules to be executed.</param>
    /// <returns>An AppliedRule object representing the outcome of the rule evaluation, including details about which rules were
    /// applied and their results.</returns>
    public AppliedRule ApplyRules(string Context, RulesToApply RulesToApply)
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
