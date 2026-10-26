using RightRule.RuleServices.RuleModels;

namespace RightRule.Factories.RuleLoaderFactory;

internal interface IRuleLoader
{
    List<DefinedRule>? LoadFromJson(string JsonData);
}
