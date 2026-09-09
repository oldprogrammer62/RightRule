using RightRule;
using RightRule.Builders.RightRule;
using RightRule.Builders.RightRuleBuilder;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RuleApplication;
using RightRule.SyntaxChecker;
using RightRule.System.Common;

namespace BeSoft;

public interface IRightRule
{
    string GetConfigSettings(string key);

    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    /// <summary>
    /// Returns the set of Logic Operators currently loaded in the RightRule engine.
    /// </summary>
    /// <returns>The list of Operators.</returns>
    List<Operator>? GetOperatorDetails();

    /// <summary>
    /// Returns the set of Aliases for currently loaded Logic Operators
    /// </summary>
    /// <returns>The list of Operator Aliases.</returns>
    Dictionary<string, Operator>? GetOperatorAliases();

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, the Default rule will be returned.
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
    Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, the Default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    RightRuleResult<IProcessedRule> ApplyRules(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="RulesToReturn">Used to indicate if the results should be first positive rule or all rules</param>
    List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, RulesToReturn RulesToReturn);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="RulesToReturn">Used to indicate if the results should be first positive rule or all rules</param>
    Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, RulesToReturn RulesToReturn);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s) and the additional data.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, the Default rule will be returned.<br/>
    /// The additional data will be discarded after this rule application
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">Name of additional Data Set</param>
    /// <param name="Data">Additional Data Set</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    RightRuleResult<IProcessedRule> ApplyRules(string Context, string DataName, Dictionary<string, object> Data);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s) and the additional data.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, the Default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">Name of additional Data Set</param>
    /// <param name="Data">Additional Data Set</param>
    /// <param name="DataRetention">Keep or discard the additional data</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    RightRuleResult<IProcessedRule> ApplyRules(string Context, string DataName, Dictionary<string, object> Data, DataRetention DataRetention);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s) and the additional data.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, the Default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">Name of additional Data Set</param>
    /// <param name="Data">Additional Data Set</param>
    /// <param name="DataRetention">Keep or discard the additional data</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, DataRetention DataRetention);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s) and the additional data.<br/>
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">Name of additional Data Set</param>
    /// <param name="Data">Additional Data Set</param>
    /// <param name="RulesToReturn"></param>
    /// <param name="DataRetention">Keep or discard the additional data</param>
    /// <returns>
    /// All Rules will be returned<br/>
    /// </returns>
    List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn RulesToReturn, DataRetention DataRetention);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s) and the additional data.<br/>
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">Name of additional Data Set</param>
    /// <param name="Data">Additional Data Set</param>
    /// <param name="RulesToReturn"></param>
    /// <param name="DataRetention">Keep or discard the additional data</param>
    /// <returns>
    /// All Rules will be returned<br/>
    /// </returns>
    Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn RulesToReturn, DataRetention DataRetention);

    /// <summary>
    /// DeRegisters all data from the RightRule engine.
    /// </summary>
    bool DeRegisterData();

    /// <summary>
    /// DeRegisters the specified data from the RightRule engine.
    /// </summary>
    /// <param name="DataName">The name of the dataset to deregister.</param>
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
    bool DeRegisterRules(string Context);

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Name">The Name of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    bool DeRegisterRule(string Name);

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Number">The Number of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    bool DeRegisterRule(int Number);

    /// <summary>
    /// Returns the set of Registered Datasets.
    /// </summary>
    /// <returns>RegisteredDatasets object</returns>
    IRegisteredDatasets GetRegisteredDatasets();

    /// <summary>
    /// Returns the set of Registered DataObjects
    /// </summary>
    /// <returns>RegisteredDataObjects object</returns>
    IRegisteredDataObjects GetRegisteredDataObjects();

    /// <summary>
    /// Returns the specified Dataset.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <returns>RegisteredDataset object</returns>
    RegisteredDataset? GetRegisteredDataset(string DataName);

    /// <summary>
    /// Returns the specified DataObject
    /// </summary>
    /// <param name="DataName">The name of the dataobject.</param>
    /// <returns>RegisteredDataObject</returns>
    RegisteredDataObject? GetRegisteredDataObject(string DataName);

    /// <summary>
    /// Returns the Registered Rules with the specified Name.
    /// </summary>
    /// <param name="Name"></param>
    ParsedRule? GetRegisteredRule(string Name);

    /// <summary>
    /// Returns the Registered Rules with the specified Number.
    /// </summary>
    /// <param name="Number"></param>
    ParsedRule? GetRegisteredRule(int Number);

    /// <summary>
    /// Returns the set of Registered Rules.
    /// </summary>
    IRegisteredRules GetRegisteredRules();

    /// <summary>
    /// Returns the set of Registered Rules for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    List<ParsedRule> GetRegisteredRules(string Context);

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="Data">Dictionary of data to register.</param>
    /// <returns>DataRegistrationResult</returns>
    DataRegistrationResult RegisterData(string DataName, Dictionary<string, object> Data);

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="DataReference">Object reference of data to register.</param>
    /// <returns>DataRegistrationResult</returns>
    DataRegistrationResult RegisterData(string DataName, object DataReference);

    /// <summary>
    /// Returns the set of Registered Default Rule for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    DefaultRule GetDefaultRule(string Context);

    /// <summary>
    /// Build a Default Rule definition<br></br>
    /// Used when a single rule result is requested, but no rule evaluated a true result
    /// </summary>
    IDefaultNameStep BuildDefaultRule();

    /// <summary>
    /// Registers a Default Rule with the RightRule engine.<br/>
    /// The Rule parameter will define the Context for the Default Rule registration.
    /// </summary>
    /// <param name="Rule">The Rule definition to register.</param>
    DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule);

    /// <summary>
    /// Build a Standard Rule definition
    /// </summary>
    INumberStep BuildRule();

    /// <summary>
    /// Registers a Rule with the RightRule engine.
    /// </summary>
    /// <param name="Rule">The Rule definition to register.</param>
    RuleRegistrationResult RegisterRule(DefinedRule Rule);

    /// <summary>
    /// Registers a set of Rules with the RightRule engine.
    /// </summary>
    /// <param name="rules">The Rule definitions to register.</param>
    /// <returns>The set of Parsed Rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> rules);

    /// <summary>
    /// Registers a set of Rules with the RightRule engine from a JSON string.
    /// </summary>
    /// <param name="JsonData">The Rule definitions to register.</param>
    /// <returns>The set of Parsed Rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    List<RuleRegistrationResult> RegisterRulesJson(string JsonData);

    /// <summary>
    /// Unloads an Operator from the RightRule engine.<br/>
    /// The Operator will no longer be valid in a Rule Statement
    /// </summary>
    /// <param name="OperatorName">The name of the Operator to unload.</param>
    /// <exception cref="ArgumentException"></exception>
    bool UnloadOperator(string OperatorName);

    /// <summary>
    /// Get the set of Rule Numbers registered
    /// </summary>
    List<(int Number, string Name)>? GetRuleNumbers();

    /// <summary>
    /// Get the set of Rule Names registered
    /// </summary>
    List<string>? GetRuleNames();

    /// <summary>
    /// Get the set of Contexts for which rules are registered
    /// </summary>
    List<string>? GetRuleContexts();
}
