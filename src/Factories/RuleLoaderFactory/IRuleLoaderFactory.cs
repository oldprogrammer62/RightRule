using RightRule.RuleServices.RuleLoader;

namespace RightRule.Factories.RuleLoaderFactory;

internal interface IRuleLoaderFactory
{
    IRuleLoader? BuildRuleLoader(string LoaderType);
}