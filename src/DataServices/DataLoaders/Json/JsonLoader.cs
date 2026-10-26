using Newtonsoft.Json;
using RightRule.Factories.RuleLoaderFactory;
using RightRule.RuleServices.RuleModels;

namespace RightRule.DataServices.DataLoaders.Json;

internal sealed class JsonLoader : IRuleLoader
{
    public JsonLoader()
    {
    }

    public List<DefinedRule>? LoadFromJson(string JsonData)
    {
        var ruleDefs = ConvertJson(JsonData);
        return ruleDefs;
    }

    private List<DefinedRule>? ConvertJson(string JsonData)
    {
        if (string.IsNullOrWhiteSpace(JsonData))
        {
            return null;
        }

        try
        {
            List<DefinedRule>? rules = JsonConvert.DeserializeObject<List<DefinedRule>>(JsonData);
            return rules;
        }
        catch
        {
            return null;
        }
    }
}
