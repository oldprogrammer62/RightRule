using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.RegistrationServices.RuleRegistration;

internal interface IRuleService
{
    IRegisteredRules RegisteredRules { get; }

    List<ParsedRule> GetRules();
    List<ParsedRule> GetRules(string Context);

    ParsedRule? GetRule(string Name);
    ParsedRule? GetRule(int Number);

    List<(int Number, string Name)>? GetRuleNumbers();
    List<string>? GetRuleNames();
    List<string>? GetRuleContexts();

    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames);
    RuleRegistrationResult RegisterRule(DefinedRule rule, HashSet<string> OperatorNames);
    bool DeRegisterRules(string Context);
    bool DeRegisterRules();
    bool DeRegisterRule(string Name);
    bool DeRegisterRule(int Number);
}
