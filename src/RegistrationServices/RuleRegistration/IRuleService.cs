using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.RegistrationServices.RuleRegistration;

internal interface IRuleService
{
    IRegisteredRules RegisteredRules { get; }

    List<ParsedRule> GetRules();
    List<ParsedRule> GetRules(string Context);

    ParsedRule? GetRule(string RuleName);
    ParsedRule? GetRule(int RuleNumber);

    List<(int Number, string Name)>? GetRuleNumbers();
    List<string>? GetRuleNames();
    List<string>? GetRuleContexts();

    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules);
    RuleRegistrationResult RegisterRule(DefinedRule Rule);

    bool DeRegisterRules();
    bool DeRegisterRules(string Context);
    bool DeRegisterRule(string Context, string RuleName);
    bool DeRegisterRule(string Context, int RuleNumber);
}
