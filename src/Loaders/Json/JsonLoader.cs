using BeSoft;
using Newtonsoft.Json;
using RightRule.Loaders.RuleLoader;
using RightRule.Models.RuleModels;

namespace RightRule.Loaders.Json;

internal class JsonLoader : IRuleLoader
{
    private readonly IRightRule _rightRule;

    public JsonLoader(IRightRule rightRule)
    {
        _rightRule = rightRule;
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
