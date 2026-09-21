using RightRule.Models.RuleModels;

namespace RightRule.Loaders.RuleLoader;

public interface IRuleLoader
{
    List<DefinedRule>? LoadFromJson(string JsonData);
}
