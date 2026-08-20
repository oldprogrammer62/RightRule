using RightRule.RuleModels;

namespace RightRule.RuleRegistration;

public interface IRegisteredRules
{
    List<ParsedRule> Rules { get; }
    int Count { get; }
    ParsedRule? this[int index] { get; }
    void AddRule(ParsedRule rule);
    void AddRules(List<ParsedRule> rules);
    bool RemoveRules(string Context);
    bool RemoveRule(string Name);
    bool RemoveRule(int Number);
    bool RemoveRules();
    List<string> GetRuleNames();
    List<string> GetRuleContexts();
    List<(int Number, string Name)> GetRuleNumbers();
}
