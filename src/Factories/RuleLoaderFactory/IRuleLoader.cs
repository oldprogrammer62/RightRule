using RightRule.RuleServices.RuleModels;

namespace RightRule.Factories.RuleLoaderFactory;

internal interface IRuleLoader
{
    List<DefinedRule>? Load(string JsonData);
}
