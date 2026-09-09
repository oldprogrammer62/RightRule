using RightRule.Models.RuleModels;

namespace RightRule.Loaders.RuleLoader;

public interface IRuleLoader
{
    List<DefinedRule>? Load(string Data);
}
