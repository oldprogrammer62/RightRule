using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;

namespace RightRule.RuleServices.Rules;

public interface IRegisteredRules
{
    List<ParsedRule> Rules { get; }
    int Count { get; }

    ParsedRule? this[int index] { get; }

    RuleRegistrationResult AddRule(ParsedRule Rule);

    bool RemoveRules();
    bool RemoveRules(string Context);
    bool RemoveRule(string Context, string RuleName);
    bool RemoveRule(string Context, int RuleNumber);

    List<string> GetRuleContexts();
    List<string>? GetRuleNames(string Context);
    List<(int Number, string Name)>? GetRuleNumbers(string Context);
}
