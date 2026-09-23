using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.RuleServices.Rules;

namespace RightRule.RuleServices;

public interface IRuleService
{
    IRegisteredRules RegisteredRules { get; }

    List<ParsedRule> GetRules();
    List<ParsedRule> GetRules(string Context);
    ParsedRule? GetRule(string Context, string RuleName);
    ParsedRule? GetRule(string Context, int RuleNumber);

    List<string>? GetRuleContexts();
    List<(int Number, string Name)>? GetRuleNumbers(string Context);
    List<string>? GetRuleNames(string Context);

    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules);
    RuleRegistrationResult RegisterRule(DefinedRule Rule);

    bool DeRegisterRules();
    bool DeRegisterRules(string Context);
    bool DeRegisterRule(string Context, string RuleName);
    bool DeRegisterRule(string Context, int RuleNumber);
}
