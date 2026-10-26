namespace RightRule.Factories.RuleLoaderFactory;

internal interface IRuleLoaderFactory
{
    IRuleLoader? BuildLoader(string LoaderType);
}