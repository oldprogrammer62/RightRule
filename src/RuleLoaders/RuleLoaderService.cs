using BeSoft;
using RightRule.RuleModels;

namespace RightRule.RuleLoaders;

internal static class RuleLoaderService
{
    public static List<RuleDefinition>? LoadRulesFromJson(IRightRule rightRule, string JsonData)
    {
        string loaderType = "JSON";
        var loader = GetLoader(rightRule, loaderType);
        if (loader == null)
        {
            return null;
        }

        var rules = loader.Load(JsonData);
        return rules;
    }

    private static IRuleLoader GetLoader(IRightRule rightRule, string LoaderType)
    {
        var loaderFactory = new Factories.Loaders.LoaderFactory(rightRule);
        var loader = loaderFactory.BuildLoader(LoaderType);
        return loader;
    }
}
