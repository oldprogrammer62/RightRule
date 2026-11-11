using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleLoader;

internal interface IRuleLoader
{
    List<DefinedRule>? Deserialize(string JsonRules);
}
