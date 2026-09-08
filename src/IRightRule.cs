using RightRule.Builders.RightRule;
using RightRule.Builders.RightRuleBuilder;
using RightRule.Models;
using RightRule.Models.DataRegistration;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.Rules;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
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
    /// Build an enhanced Rule Definition
    /// Based on Interface to force fiuelds to be applied to the rule defn in order
    /// </summary>
    INumberStep BuildRule();

    IDefaultNameStep BuildDefaultRule();

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
    Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync(string Context);

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
    RightRuleResult<IProcessedRule> ApplyRules(string Context);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="rulesToReturn"></param>
    /// <returns></returns>
    List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, RulesToReturn rulesToReturn);
    Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, RulesToReturn rulesToReturn);

    RightRuleResult<IProcessedRule> ApplyRules(string Context, string DataName, Dictionary<string, object> Data);
    RightRuleResult<IProcessedRule> ApplyRules(string Context, string DataName, Dictionary<string, object> Data, DataRetention dataRetention);
    Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, DataRetention dataRetention);

    List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn, DataRetention dataRetention);
    Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn, DataRetention dataRetention);


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
    IRegisteredRules RegisteredRules();

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
    DataRegistrationResult RegisterData(string DataName, Dictionary<string, object> Data);

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="DataReference">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    DataRegistrationResult RegisterData(string DataName, object DataReference);

    DefaultRule GetDefaultRule(string Context);

    DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule rule);

    /// <summary>
    /// Registers a single Rule with the RightRule engine.
    /// </summary>
    /// <param name="rule">The Rule definition to register.</param>
    /// <returns>The Parsed Rule.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    RuleRegistrationResult RegisterRule(DefinedRule rule);

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
    /// Unloads an Operator from the RightRule engine.
    /// The Operator will no longer be valid in a Rule Statement
    /// </summary>
    /// <param name="OperatorName">The name of the Operator to unload.</param>
    /// <exception cref="ArgumentException"></exception>
    bool UnloadOperator(string OperatorName);

    List<(int Number, string Name)>? GetRuleNumbers();

    List<string>? GetRuleNames();

    List<string>? GetRuleContexts();
}
