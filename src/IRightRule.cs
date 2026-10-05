using RightRule.DataProviders;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleBuilders.DefaultRuleBuilder;
using RightRule.RuleServices.RuleBuilders.RightRuleBuilder;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.System.Common;

namespace RightRule;

public interface IRightRule : IDisposable
{
    IRuleService RuleService { get; }

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
    Task<RightRuleResult<IProcessedRule>> ApplyRuleAsync(string Context);

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
    RightRuleResult<IProcessedRule> ApplyRule(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// EvaluationType will default to First
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    List<RightRuleResult<IProcessedRule>> ApplyRules(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// EvaluationType will default to First
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="EvaluationType">Used to indicate if the results should be first positive rule or all rules</param>
    List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, EvaluationTypeEnum EvaluationType);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="EvaluationType">Used to indicate if the results should be first positive rule or all rules</param>
    Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum EvaluationType);

    /// <summary>
    /// DeRegisters all data from the RightRule engine.
    /// </summary>
    bool DeRegisterData();

    /// <summary>
    /// DeRegisters the specified data from the RightRule engine.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset to deregister.</param>
    bool DeRegisterData(string DatasetName);

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
    /// <param name="Context"></param>
    /// <param name="RuleName">The Name of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    bool DeRegisterRule(string Context, string RuleName);

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleNumber">The Number of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    bool DeRegisterRule(string Context, int RuleNumber);

    List<Dataset>? GetRegisteredData();

    Dataset? GetRegisteredData(string DatasetName);

    /// <summary>
    /// Returns the set of Registered Rules.
    /// </summary>
    List<ParsedRule> GetRegisteredRules();

    /// <summary>
    /// Returns the Registered Rules with the specified Name.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleName"></param>
    ParsedRule? GetRegisteredRule(string Context, string RuleName);

    /// <summary>
    /// Returns the Registered Rules with the specified Number.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleNumber"></param>
    ParsedRule? GetRegisteredRule(string Context, int RuleNumber);

    /// <summary>
    /// Returns the set of Registered Rules for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    List<ParsedRule> GetRegisteredRules(string Context);

    /// <summary>
    /// Registers an external data provider with the RightRule engine.<br/>
    /// </summary>
    /// <param name="DataProvider"></param>
    /// <returns></returns>
    bool RegisterDataProvider(object DataProvider);

    /// <summary>
    /// NEW
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="DatasetName"></param>
    /// <param name="Data"></param>
    /// <returns></returns>
    DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull;

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="DataItems">Dictionary of data to register.</param>
    /// <returns>DataRegistrationResult</returns>
    DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems);

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="DataItem">Object reference of data to register.</param>
    /// <returns>DataRegistrationResult</returns>
    DataRegistrationResult RegisterData(string DatasetName, ValueTuple<string, object> DataItem);
    List<DataRegistrationResult> RegisterData(string DatasetName, List<ValueTuple<string, object>> DataItems);

    DataRegistrationResult RegisterData(string DataItemName, object DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, bool DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, string DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, int DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, long DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, double DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, decimal DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, DateTime DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, DateOnly DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, TimeOnly DataItemValue);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue);

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
    /// <param name="Rules">The Rule definitions to register.</param>
    /// <returns>The set of Parsed Rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules);

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
    /// Get the set of Contexts for which rules are registered
    /// </summary>
    List<string>? GetRuleContexts();

    /// <summary>
    /// Get the set of Rule Numbers registered
    /// </summary>
    List<(int Number, string Name)>? GetRuleNumbers(string Context);

    /// <summary>
    /// Get the set of Rule Names registered
    /// </summary>
    List<string>? GetRuleNames(string Context);
}
