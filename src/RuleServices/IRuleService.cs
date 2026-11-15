using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleBuilders.DefaultRuleBuilder;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;

namespace RightRule.RuleServices;

public interface IRuleService
{
    IRegisteredRules RegisteredRules { get; }

    List<ParsedRule> GetRegisteredRules();
    List<ParsedRule> GetRegisteredRules(string Context);
    ParsedRule? GetRegisteredRule(string Context, string RuleName);
    ParsedRule? GetRegisteredRule(string Context, int RuleNumber);

    List<string>? GetRuleContexts();
    List<(int Number, string Name)>? GetRuleNumbers(string Context);
    List<string>? GetRuleNames(string Context);

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
}
