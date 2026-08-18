using RightRule.RuleModels;

namespace RightRule.RuleLoaders;

public interface IRuleLoader
{
    List<ParsedRule> Load();

    List<ParsedRule> Load(string Data);
}
