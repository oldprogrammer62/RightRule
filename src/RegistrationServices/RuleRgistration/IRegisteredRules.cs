using RightRule.RegistrationServices.RuleRgistration;
using RightRule.RuleModels.RuleModels;

namespace RightRule.RegistrationServices.Rules;

public interface IRegisteredRules
{
    List<ParsedRule> Rules { get; }
    int Count { get; }

    ParsedRule? this[int index] { get; }

    RuleRegistrationResult AddRule(ParsedRule rule);
    List<RuleRegistrationResult> AddRules(List<ParsedRule> rules);

    bool RemoveRules(string Context);
    bool RemoveRule(string Name);
    bool RemoveRule(int Number);
    bool RemoveRules();

    List<string> GetRuleNames();
    List<string> GetRuleContexts();
    List<(int Number, string Name)> GetRuleNumbers();
}
