using RightRule.RuleModels;

namespace RightRule.RuleLoaders;

public interface IRuleLoader
{
    List<RuleDefinition>? Load(string Data);
}
