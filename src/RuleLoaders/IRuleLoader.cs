using RightRule.RuleModels.RuleModels;

namespace RightRule.RuleLoaders;

public interface IRuleLoader
{
    List<DefinedRule>? Load(string Data);
}
