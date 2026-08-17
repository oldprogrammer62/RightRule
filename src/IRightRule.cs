using RightRule.Data;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.SyntaxChecker;
using RightRule.System.Common;
using RightRule.System.Common.Exceptions;

namespace BeSoft;

public interface IRightRule
{
    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    /// <summary>
    /// Build an enhanced Rule Definition
    /// Based on Interface to force fiuelds to be applied to the rule defn in order
    /// </summary>
    INumberStep BuildRuleDefinition();

    RightRuleConfiguration RightRuleConfiguration { get; }

    /// <summary>
    /// Returns the set of Operators currently loaded in the RightRule engine.
    /// </summary>
    /// <returns>The list of Operators.</returns>
    List<Operator>? GetOperatorDetails();

    /// <summary>
    /// Returns the set of Aliases for logic Operators
    /// </summary>
    /// <returns>The list of Operator Aliases.</returns>
    Dictionary<string, Operator>? GetOperatorAliases();

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    Task<AppliedRule> EvaluateFirstRuleAsync(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    AppliedRule EvaluateFirstRule(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given context to the data set provided.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    Task<AppliedRule> EvaluateFirstRuleAsync(string Context, string DataName, Dictionary<string, object> Data);

    /// <summary>
    /// Apply all Registered Rules for the given context to the data set provided.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    AppliedRule EvaluateFirstRule(string Context, string DataName, Dictionary<string, object> Data);

    /// <summary>
    /// Apply all Registered Rules for the given context to the registered data set(s).<br/>
    /// The results of all applied rules will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// All Rules applied are returned in a list.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given context to the registered data set(s).<br/>
    /// The results of all applied rules will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// All Rules applied are returned in a list.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    List<AppliedRule> EvaluateAllRules(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given context to the registered data set(s).<br/>
    /// The results returned are defined by the setting of RulesToReturn<br/>
    /// <b>First</b> will return a single Rule result<br/>
    /// <b>All</b> will return results of all Rules.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="ReturnRules">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified ReturnRules parameter.
    /// </returns>
    Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, RulesToReturn ReturnRules);

    /// <summary>
    /// Apply all Registered Rules for the given context to the registered data set(s).<br/>
    /// The results returned are defined by the setting of RulesToReturn<br/>
    /// <b>First</b> will return a single Rule result<br/>
    /// <b>All</b> will return results of all Rules.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="ReturnRules">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified ReturnRules parameter.
    /// </returns>
    List<AppliedRule> EvaluateAllRules(string Context, RulesToReturn ReturnRules);

    /// <summary>
    /// Apply all Registered Rules for the given context to the specified data set.<br/>
    /// The results of all applied rules will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the specified data set.<br/>
    /// The results of all applied rules will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    List<AppliedRule> EvaluateAllRules(string Context, string DataName, Dictionary<string, object> Data);

    /// <summary>
    /// Apply all Registered Rules for the given context to the specified data set.<br/>
    /// The results returned are defined by the setting of RulesToReturn<br/>
    /// <b>First</b> will return a single Rule result<br/>
    /// <b>All</b> will return results of all Rules.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DataName">The name of the registered dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <param name="ReturnRules">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified ReturnRules parameter.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules);

    /// <summary>
    /// Apply all Registered Rules for the given context to the specified data set.<br/>
    /// The results returned are defined by the setting of RulesToReturn<br/>
    /// <b>First</b> will return a single Rule result<br/>
    /// <b>All</b> will return results of all Rules.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DataName">The name of the registered dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <param name="ReturnRules">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified ReturnRules parameter.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    List<AppliedRule> EvaluateAllRules(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules);

    /// <summary>
    /// DeRegisters all data from the RightRule engine.
    /// </summary>
    bool DeRegisterData();

    /// <summary>
    /// DeRegisters the specified data from the RightRule engine.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <exception cref="ArgumentException"></exception>
    bool DeRegisterData(string DataName);

    /// <summary>
    /// Deregisters all Rules from the RightRule engine.
    /// </summary>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    bool DeRegisterRules();

    /// <summary>
    /// Deregisters all Rules associated with the specified Context from the RightRule engine.
    /// </summary>
    /// <param name="Context">The Context for which to deregister rules.</param>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    bool DeRegisterRules(string Context);

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Name">The Name of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    bool DeRegisterRule(string Name);

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Number">The Number of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    bool DeRegisterRule(int Number);

    /// <summary>
    /// Returns the set of Registered Datasets.
    /// </summary>
    /// <returns>RegisteredDatasets object</returns>
    IRegisteredDatasets RegisteredDatasets();

    /// <summary>
    /// Returns the set of Registered DataObjects
    /// </summary>
    /// <returns>RegisteredDataObjects object</returns>
    IRegisteredDataObjects RegisteredDataObjects();

    /// <summary>
    /// Returns the specified Dataset.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <returns>RegisteredDataset object</returns>
    /// <exception cref="ArgumentException"></exception>
    RegisteredDataset? RegisteredDataset(string DataName);

    /// <summary>
    /// Returns the specified DataObject
    /// </summary>
    /// <param name="DataName">The name of the dataobject.</param>
    /// <returns>RegisteredDataObject</returns>
    /// <exception cref="ArgumentException"></exception>
    RegisteredDataObject? RegisteredDataObject(string DataName);

    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Name.
    /// </summary>
    /// <param name="Name"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    ParsedRule? RegisteredRule(string Name);

    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Number.
    /// </summary>
    /// <param name="Number"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    ParsedRule? RegisteredRule(int Number);

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules.
    /// </summary>
    RegisteredRules RegisteredRules();

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    /// <exception cref="ArgumentException"></exception>
    List<ParsedRule> RegisteredRules(string Context);

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="Data">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    bool RegisterData(string DataName, Dictionary<string, object> Data);

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="DataReference">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    bool RegisterData(string DataName, object DataReference);

    /// <summary>
    /// Registers a single Rule with the RightRule engine.
    /// </summary>
    /// <param name="rule">The Rule definition to register.</param>
    /// <returns>The Parsed Rule.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    ParsedRule RegisterRule(RuleDefinition rule);

    /// <summary>
    /// Registers a set of Rules with the RightRule engine.
    /// </summary>
    /// <param name="rules">The Rule definitions to register.</param>
    /// <returns>The set of Parsed Rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules);

    /// <summary>
    /// Unloads an Operator from the RightRule engine.
    /// The Operator will no longer be valid in a Rule Statement
    /// </summary>
    /// <param name="OperatorName">The name of the Operator to unload.</param>
    /// <exception cref="ArgumentException"></exception>
    bool UnloadOperator(string OperatorName);
}