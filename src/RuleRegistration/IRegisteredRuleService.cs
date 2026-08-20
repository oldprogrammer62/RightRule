using RightRule.RuleModels;

namespace RightRule.RuleRegistration;

internal interface IRegisteredRuleService
{
    IRegisteredRules RegisteredRules { get; }
    ParsedRule? GetRegisteredRule(string Name);
    ParsedRule? GetRegisteredRule(int Number);
    List<ParsedRule> GetRules(string Context);

    List<(int Number, string Name)>? GetRuleNumbers();
    List<string>? GetRuleNames();
    List<string>? GetRuleContexts();

    List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
    ParsedRule? RegisterRule(RuleDefinition rule, HashSet<string> OperatorNames, HashSet<string>? ObjectNames, HashSet<string>? DatasetNames);
    bool DeRegisterRules(string Context);
    bool DeRegisterRules();
    bool DeRegisterRule(string Name);
    bool DeRegisterRule(int Number);
}
