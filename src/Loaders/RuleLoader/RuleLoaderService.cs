using BeSoft;
using RightRule.Models.RuleModels;

namespace RightRule.Loaders.RuleLoader;

internal static class RuleLoaderService
{
    public static List<DefinedRule>? LoadRulesFromJson(IRightRule RightRule, string JsonData)
    {
        string loaderType = "JSON";
        var loader = GetLoader(RightRule, loaderType);
        if (loader == null)
        {
            return null;
        }

        var rules = loader.LoadFromJson(JsonData);
        return rules;
    }

    private static IRuleLoader GetLoader(IRightRule RightRule, string LoaderType)
    {
        var loaderFactory = new Factories.RuleLoaderFactory.RuleLoaderFactory(RightRule);
        var loader = loaderFactory.BuildLoader(LoaderType);
        return loader;
    }
}
