using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleBuilders.DefaultRuleBuilder;
using RightRule.RuleServices.RuleBuilders.RightRuleBuilder;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;

namespace RightRule.RuleServices;

public interface IRuleService
{
    IRegisteredRules RegisteredRules { get; }
    List<DefaultRulePerContext> DefaultRules { get; }
    List<string> RuleContexts { get; }

    INumberStep BuildRule();

    List<ParsedRule> GetRegisteredRules();
    List<ParsedRule> GetRegisteredRules(string Context);
    ParsedRule? GetRegisteredRule(string Context, string RuleName);
    ParsedRule? GetRegisteredRule(string Context, int RuleNumber);

    List<string> GetRuleContexts();
    List<(int Number, string Name)> GetRuleNumbers(string Context);
    List<string> GetRuleNames(string Context);

    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules);
    RuleRegistrationResult RegisterRule(DefinedRule Rule);

    List<RuleRegistrationResult> LoadRules(string JsonData);

    bool DeRegisterRules();
    bool DeRegisterRules(string Context);
    bool DeRegisterRule(string Context, string RuleName);
    bool DeRegisterRule(string Context, int RuleNumber);

    string SerializeRules();
    string SerializeRules(string Context);

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

    DefaultRule GetDefaultRule(string Context);

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
}
