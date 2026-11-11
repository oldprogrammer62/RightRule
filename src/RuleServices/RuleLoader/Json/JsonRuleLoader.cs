using Newtonsoft.Json;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleLoader.Json;

internal sealed class JsonRuleLoader : IRuleLoader
{
    public JsonRuleLoader()
    {
    }

    public List<DefinedRule>? Deserialize(string JsonRules)
    {
        var ruleDefs = ConvertJson(JsonRules);
        return ruleDefs;
    }

    private static List<DefinedRule>? ConvertJson(string JsonRules)
    {
        if (string.IsNullOrWhiteSpace(JsonRules))
        {
            return null;
        }

        try
        {
            return JsonConvert.DeserializeObject<List<DefinedRule>>(JsonRules)
                ?? throw new JsonSerializationException("The JSON must contain an array of rule definitions.");
        }
        catch (JsonException jex)
        {
            throw new ArgumentException(
                "The input must be a valid JSON array of rule definitions.",
                nameof(JsonRules),
                jex);
        }
    }
}
