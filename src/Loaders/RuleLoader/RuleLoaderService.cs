using BeSoft;
using RightRule.Models.RuleModels;

namespace RightRule.Loaders.RuleLoader;

internal static class RuleLoaderService
{
    public static List<DefinedRule>? LoadRulesFromJson(IRightRule rightRule, string JsonData)
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
        var loaderFactory = new Factories.RuleLoaderFactory.RuleLoaderFactory(rightRule);
        var loader = loaderFactory.BuildLoader(LoaderType);
        return loader;
    }
}
