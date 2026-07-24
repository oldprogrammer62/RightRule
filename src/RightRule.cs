using RightRule.Data;
using RightRule.Extensions;
using RightRule.Models;
using RightRule.Rules;
using RightRule.SyntaxChecker;
using RightRule.System.Common;
using RightRule.System.Common.Exceptions;
using System.Xml.Linq;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed class RightRule : IRightRule
{
    private readonly RightRuleService _rightRuleService;

    public RightRuleConfiguration RightRuleConfiguration => _rightRuleService.RightRuleConfig;

    public RightRule()
    {
        _rightRuleService = new RightRuleService();
    }

    public CheckStatementResult SyntaxCheck(string Statement)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Statement, nameof(Statement));
        return this.CheckSyntax(Statement);
    }

    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Name.
    /// </summary>
    /// <param name="Name"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? RegisteredRule(string Name)
    {
        return _rightRuleService.GetRegisteredRule(Name);
    }

    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Number.
    /// </summary>
    /// <param name="Number"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? RegisteredRule(int Number)
    {
        return _rightRuleService.GetRegisteredRule(Number);
    }

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules.
    /// </summary>
    public RegisteredRules RegisteredRules()
    {
        return _rightRuleService.RegisteredRules;
    }

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    /// <exception cref="ArgumentException"></exception>
    public List<ParsedRule> RegisteredRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return _rightRuleService.GetRules(Context);
    }

    /// <summary>
    /// Registers a set of Rules with the RightRule engine.
    /// </summary>
    /// <param name="rules">The Rule definitions to register.</param>
    /// <returns>The set of Parsed Rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
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
    /// Registers a single Rule with the RightRule engine.
    /// </summary>
    /// <param name="rule">The Rule definition to register.</param>
    /// <returns>The Parsed Rule.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
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
    /// Deregisters all Rules associated with the specified Context from the RightRule engine.
    /// </summary>
    /// <param name="Context">The Context for which to deregister rules.</param>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        return _rightRuleService.DeRegisterRules(Context);
    }

    /// <summary>
    /// Deregisters all Rules from the RightRule engine.
    /// </summary>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    public bool DeRegisterRules()
    {
        return _rightRuleService.DeRegisterRules();
    }

    /// <summary>
    /// Deregister all specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Name">The Name of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRule(string Name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name, nameof(Name));
        return _rightRuleService.DeRegisterRule(Name);
    }

    /// <summary>
    /// Deregister all specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Number">The Number of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRule(int Number)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Number.ToString(), nameof(Number));
        return _rightRuleService.DeRegisterRule(Number);
    }

    /// <summary>
    /// Returns the specified Dataset.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <returns>Registered Dataset</returns>
    /// <exception cref="ArgumentException"></exception>
    public RegisteredDataset? RegisteredDataset(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        return _rightRuleService.GetDataSet(DatasetName);
    }

    /// <summary>
    /// Returns the set of Registered Datasets.
    /// </summary>
    /// <returns>RegisteredDatasets object</returns>
    public RegisteredDatasets RegisteredDatasets()
    {
        return _rightRuleService.RegisteredDatasets;
    }


    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="Data">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public bool RegisterDataset(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        try
        {
            return _rightRuleService.RegisterDataset(DatasetName, Data);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// DeRegisters all data sets from the RightRule engine.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterDatasets()
    {
        try
        {
            _rightRuleService.DeRegisterDatasets();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// DeRegisters a data set from the RightRule engine.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterDataset(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        try
        {
            _rightRuleService.DeRegisterDataset(DatasetName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Unloads an Operator from the RightRule engine.
    /// The Operator will no longer be valid in a Rule Statement
    /// </summary>
    /// <param name="OperatorName">The name of the Operator to unload.</param>
    /// <exception cref="ArgumentException"></exception>
    public bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));

        try
        {
            return _rightRuleService.UnloadOperator(OperatorName);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Apply all registered Rules for the given Context to the registered data set(s).
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidIdentifierException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public AppliedRule ApplyRule(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var appliedRules = _rightRuleService.ApplyRule(Context);
            if (appliedRules == null)
            {
                throw new InvalidOperationException("Rule not applied");
            }
            if (appliedRules.Count == 1)
            {
                return appliedRules[0];
            }
            return AppliedRule.DefaultRule();
        }
        catch (Exception ex)
        {
            return AppliedRule.DefaultRule(ex.Message);
        }
    }

    /// <summary>
    /// Apply all registered rules for the given context to the data set provided.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DatasetName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    public AppliedRule ApplyRule(string Context, string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        var appliedRules = _rightRuleService.ApplyRule(Context, DatasetName, Data);
        if (appliedRules == null)
        {
            throw new InvalidOperationException("Rule not applied");
        }
        if (appliedRules.Count == 1)
        {
            return appliedRules[0];
        }
        return AppliedRule.DefaultRule();
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
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var appliedRules = _rightRuleService.ApplyRules(Context);
        if (appliedRules == null)
        {
            throw new InvalidOperationException("Rules not applied");
        }
        if (appliedRules.Count > 0)
        {
            return appliedRules;
        }
        return [AppliedRule.DefaultRule()];
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
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var appliedRules = _rightRuleService.ApplyRules(Context, rulesToReturn);
        if (appliedRules == null)
        {
            throw new InvalidOperationException("Rules not applied");
        }
        if (appliedRules.Count > 0)
        {
            return appliedRules;
        }
        return [AppliedRule.DefaultRule()];
    }

    /// <summary>
    /// Apply all registered rules for the given context to the specified data set.
    /// </summary>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DatasetName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    public List<AppliedRule> ApplyRules(string Context, string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        var appliedRules = _rightRuleService.ApplyRules(Context, DatasetName, Data);
        if (appliedRules == null)
        {
            throw new InvalidOperationException("Rules not applied");
        }
        if (appliedRules.Count > 0)
        {
            return appliedRules;
        }
        return [AppliedRule.DefaultRule()];
    }

    /// <summary>
    /// Apply all registered rules for the given context to the specified data set.
    /// </summary>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DatasetName">The name of the registered dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <param name="rulesToReturn">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified rulesToReturn parameter.
    /// </returns>
    public List<AppliedRule> ApplyRules(string Context, string DatasetName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        var appliedRules = _rightRuleService.ApplyRules(Context, DatasetName, Data, rulesToReturn);
        if (appliedRules == null)
        {
            throw new InvalidOperationException("Rules not applied");
        }
        if (appliedRules.Count > 0)
        {
            return appliedRules;
        }
        return [AppliedRule.DefaultRule()];
    }
}
