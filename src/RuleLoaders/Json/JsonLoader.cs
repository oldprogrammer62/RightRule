using BeSoft;
using Newtonsoft.Json;
using RightRule.RuleModels;

namespace RightRule.RuleLoaders.Json;

internal class JsonLoader : IRuleLoader
{
    private readonly IRightRule _rightRule;

    public JsonLoader(IRightRule rightRule)
    {
        _rightRule = rightRule;
    }

    public List<RuleDefinition>? Load(string JsonData)
    {
        var ruleDefs = ConvertJson(JsonData);
        return ruleDefs;
    }

    private List<RuleDefinition>? ConvertJson(string JsonData)
    {
        if (string.IsNullOrWhiteSpace(JsonData))
        {
            return null;
        }

        try
        {
            List<RuleDefinition> rules = JsonConvert.DeserializeObject<List<RuleDefinition>>(JsonData);
            return rules;
        }
        catch
        {
            return null;
        }
    }
}
