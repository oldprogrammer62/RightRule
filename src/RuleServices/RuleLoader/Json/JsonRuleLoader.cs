using Newtonsoft.Json;
using RightRule.Factories.RuleLoaderFactory;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleLoader.Json;

internal sealed class JsonRuleLoader : IRuleLoader
{
    public JsonRuleLoader()
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
            return JsonConvert.DeserializeObject<List<DefinedRule>>(JsonData)
                ?? throw new JsonSerializationException("The JSON must contain an array of rule definitions.");
        }
        catch (JsonException jex)
        {
            throw new ArgumentException(
                "The input must be a valid JSON array of rule definitions.",
                nameof(JsonData),
                jex);
        }
    }
}
