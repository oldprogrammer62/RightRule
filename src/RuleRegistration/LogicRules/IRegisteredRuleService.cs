using RightRule.RuleModels;

namespace RightRule.RuleRegistration.LogicRules;

internal interface IRegisteredRuleService
{
    IRegisteredRules RegisteredRules { get; }

    ParsedRule? GetRegisteredRule(string Name);
    ParsedRule? GetRegisteredRule(int Number);
    List<ParsedRule> GetRules(string Context);

    List<(int Number, string Name)>? GetRuleNumbers();
    List<string>? GetRuleNames();
    List<string>? GetRuleContexts();

    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
    RuleRegistrationResult RegisterRule(DefinedRule rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
    bool DeRegisterRules(string Context);
    bool DeRegisterRules();
    bool DeRegisterRule(string Name);
    bool DeRegisterRule(int Number);
}
