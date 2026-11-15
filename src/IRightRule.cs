using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.RuleBuilders.RightRuleBuilder;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.System.Common.DataAdaptor;

namespace RightRule;

public interface IRightRule : IDisposable
{
    RightRuleOptions GetRightRuleOptions();

    IRuleService RuleService { get; }
    IDataService DataService { get; }
    IOperatorService? OperatorService { get; }
    IDataAdaptorService? DataAdaptorService { get; }

    RightRuleOptions Options { get; }

    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see cref="CheckStatementResult"/></returns>
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

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
    /// Registers a custom Data Adaptor with the RightRule engine.<br/>
    /// </summary>
    /// <param name="DataAdaptor"></param>
    /// <returns><see cref="DataAdaptorRegistrationResult"/></returns>
    DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor DataAdaptor);

    /// <summary>
    /// Build a Standard Rule definition
    /// </summary>
    INumberStep BuildRule();
}
