using RightRule.DataAdaptors;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleBuilders.DefaultRuleBuilder;
using RightRule.RuleServices.RuleBuilders.RightRuleBuilder;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.System.Common.DataAdaptor;
using RightRule.System.Common.Operator.Models;

namespace RightRule;

public interface IRightRule : IDisposable
{
    RightRuleOptions GetRightRuleOptions();

    IRuleService RuleService { get; }

    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see cref="CheckStatementResult"/></returns>
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    /// <summary>
    /// Register a Custom Operator with the RightRule engine.<br/>
    /// </summary>
    /// <param name="Operator"></param>
    /// <returns><see cref="OperatorRegistrationResult"/></returns>
    OperatorRegistrationResult RegisterOperator(Operator Operator);

    /// <summary>
    /// Returns the set of Logic Operators currently loaded in the RightRule engine.
    /// </summary>
    /// <see cref="List{Operator}">List&lt;Operator&gt;</see>
    List<Operator>? GetOperators();

    /// <summary>
    /// Returns the set of Aliases for currently loaded Logic Operators
    /// </summary>
    /// <returns><see cref="Dictionary{TKey, TValue}">Dictionary&lt;string, Operator&gt;</see></returns>
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
    /// <see cref="RightRuleResult{IProcessedRule}">RightRuleResult&lt;IProcessedRule&gt;</see>
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
    /// <see cref="RightRuleResult{IProcessedRule}">RightRuleResult&lt;IProcessedRule&gt;</see>
    /// </returns>
    RightRuleResult<IProcessedRule> ApplyRule(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// EvaluationType will default to First
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// List&lt;RightRuleResult&lt;<see cref="IProcessedRule"/>&gt;&gt;
    /// </returns>
    List<RightRuleResult<IProcessedRule>> ApplyRules(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// EvaluationType will default to First
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// List&lt;RightRuleResult&lt;<see cref="IProcessedRule"/>&gt;&gt;
    /// </returns>
    Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="EvaluationType">Used to indicate if the results should be first positive rule or all rules</param>
    /// <returns>
    /// List&lt;RightRuleResult&lt;<see cref="IProcessedRule"/>&gt;&gt;
    /// </returns>
    List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, EvaluationTypeEnum EvaluationType);

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// All Rules will be returned<br/>
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="EvaluationType">Used to indicate if the results should be first positive rule or all rules</param>
    /// <returns>
    /// List&lt;RightRuleResult&lt;<see cref="IProcessedRule"/>&gt;&gt;
    /// </returns>
    Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum EvaluationType);

    /// <summary>
    /// DeRegisters all data from the RightRule engine.
    /// </summary>
    /// <returns>bool</returns>
    bool DeRegisterData();

    /// <summary>
    /// DeRegisters the specified data from the RightRule engine.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset to deregister.</param>
    /// <returns>bool</returns>
    bool DeRegisterData(string DatasetName);

    /// <summary>
    /// Deregisters all Rules from the RightRule engine.
    /// </summary>
    /// <returns>
    /// bool<br></br>
    /// True if the rules were successfully deregistered; otherwise, false.
    /// </returns>
    bool DeRegisterRules();

    /// <summary>
    /// Deregisters all Rules associated with the specified Context from the RightRule engine.
    /// </summary>
    /// <param name="Context">The Context for which to deregister rules.</param>
    /// <returns>
    /// bool <br></br>
    /// True if the rules were successfully deregistered; otherwise, false.</returns>
    bool DeRegisterRules(string Context);

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleName">The Name of the Rule to deregister.</param>
    /// <returns>
    /// bool<br></br>
    /// True if the rule is successfully deregistered; otherwise, false.</returns>
    bool DeRegisterRule(string Context, string RuleName);

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleNumber">The Number of the Rule to deregister.</param>
    /// <returns>
    /// bool<br></br>
    /// True if the rule is successfully deregistered; otherwise, false.</returns>
    bool DeRegisterRule(string Context, int RuleNumber);

    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// List&lt;<see cref="Dataset"/>&gt;&gt;
    /// </returns>
    List<Dataset>? GetRegisteredData();

    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// <see cref="Dataset"/>
    /// </returns>
    Dataset? GetRegisteredData(string DatasetName);

    /// <summary>
    /// Returns the set of Registered Rules.
    /// </summary>
    /// <returns>
    /// <see cref="List{ParsedRule}">List&lt;ParsedRule&gt;</see>
    /// </returns>
    List<ParsedRule> GetRegisteredRules();

    /// <summary>
    /// Returns the Registered Rules with the specified Name.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleName"></param>
    /// <returns><see cref="ParsedRule"/></returns>
    ParsedRule? GetRegisteredRule(string Context, string RuleName);

    /// <summary>
    /// Returns the Registered Rules with the specified Number.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="RuleNumber"></param>
    /// <returns><see cref="ParsedRule"/></returns>
    ParsedRule? GetRegisteredRule(string Context, int RuleNumber);

    /// <summary>
    /// Returns the set of Registered Rules for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    /// <returns><see cref="List{ParsedRule}">List&lt;ParsedRule&gt;</see></returns>
    List<ParsedRule> GetRegisteredRules(string Context);

    /// <summary>
    /// Registers a custom Data Adaptor with the RightRule engine.<br/>
    /// </summary>
    /// <param name="DataAdaptor"></param>
    /// <returns><see cref="DataAdaptorRegistrationResult"/></returns>
    DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor DataAdaptor);

    /// <summary>
    /// NEW
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="DatasetName"></param>
    /// <param name="Data"></param>
    /// <returns><see cref="DataRegistrationResult"/></returns>
    DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull;

    /// <summary>
    /// Returns the set of Registered Default Rule for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    /// <returns><see cref="DefaultRule"/></returns>
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
    /// <returns><see cref="DefaultRuleRegistrationResult"/></returns>
    DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule);

    /// <summary>
    /// Build a Standard Rule definition
    /// </summary>
    INumberStep BuildRule();

    /// <summary>
    /// Registers a Rule with the RightRule engine.
    /// </summary>
    /// <param name="Rule">The Rule definition to register.</param>
    /// <returns><see cref="RuleRegistrationResult"/></returns>
    RuleRegistrationResult RegisterRule(DefinedRule Rule);

    /// <summary>
    /// Registers a set of Rules with the RightRule engine.
    /// </summary>
    /// <param name="Rules">The Rule definitions to register.</param>
    /// <returns>
    /// <see cref = "List{RuleRegistrationResult}">List&lt;RuleRegistrationResult&gt;</see>
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules);

    /// <summary>
    /// Registers a set of Rules with the RightRule engine from a JSON string.
    /// </summary>
    /// <param name="JsonData">The Rule definitions to register.</param>
    /// <returns>
    /// <see cref = "List{RuleRegistrationResult}">List&lt;RuleRegistrationResult&gt;</see>
    /// </returns>
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
    /// <returns>
    /// <see cref="List{T}">List&lt;string&gt;</see>
    /// </returns>
    List<string>? GetRuleContexts();

    /// <summary>
    /// Get the set of Rule Numbers registered
    /// </summary>
    /// <returns>
    /// A <see cref="List{T}">List&lt;(int Number, string Name)&gt;</see> containing indexed pairs.
    /// </returns>    
    List<(int Number, string Name)>? GetRuleNumbers(string Context);

    /// <summary>
    /// Get the set of Rule Names registered
    /// </summary>
    /// <returns>
    /// <see cref="List{T}">List&lt;string&gt;</see>
    /// </returns>
    List<string>? GetRuleNames(string Context);
}
