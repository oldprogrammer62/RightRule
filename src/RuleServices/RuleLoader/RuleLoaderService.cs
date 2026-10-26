using RightRule.Factories.RuleLoaderFactory;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleLoader;

internal class RuleLoaderService : IRuleLoaderService
{
    private readonly IRuleLoaderFactory _ruleLoaderFactory;

    public RuleLoaderService(IRuleLoaderFactory ruleLoaderFactory)
    {
        _ruleLoaderFactory = ruleLoaderFactory;
    }

    public List<DefinedRule>? LoadRulesFromJson(string JsonData)
    {
        string loaderType = "JSON";
        var loader = GetLoader(loaderType);
        if (loader == null)
        {
            return [];
        }
        var rules = loader.LoadFromJson(JsonData);
        return rules;
    }

    private IRuleLoader? GetLoader(string LoaderType)
    {
        var loader = _ruleLoaderFactory.BuildLoader(LoaderType);
        return loader;
    }
}
