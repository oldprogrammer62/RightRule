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

    public List<DefinedRule>? LoadRules(string JsonData)
    {
        string loaderType = "JSON";
        var loader = GetLoader(loaderType);
        if (loader == null)
        {
            return [];
        }
        var rules = loader.Deserialize(JsonData);
        return rules;
    }

    private IRuleLoader? GetLoader(string LoaderType)
    {
        var loader = _ruleLoaderFactory.BuildRuleLoader(LoaderType);
        return loader;
    }
}
