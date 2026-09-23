using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleLoader;

public interface IRuleLoader
{
    List<DefinedRule>? LoadFromJson(string JsonData);
}
