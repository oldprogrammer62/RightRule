using Newtonsoft.Json;
using RightRule.DataServices.Data;
using RightRule.Factories.DataLoaderFactory;
using RightRule.RuleServices.RuleModels;

namespace RightData.DataServices.DataLoader.Json;

internal sealed class JsonDataLoader : IDataLoader
{
    public JsonDataLoader()
    {
    }

    public List<Dataset>? Load(string JsonData)
    {
        var ruleDefs = ConvertJson(JsonData);
        return ruleDefs;
    }

    private List<Dataset>? ConvertJson(string JsonData)
    {
        if (string.IsNullOrWhiteSpace(JsonData))
        {
            return null;
        }

        try
        {
            return JsonConvert.DeserializeObject<List<Dataset>>(JsonData)
                ?? throw new JsonSerializationException("The JSON must contain an array of dataset definitions.");
        }
        catch (JsonException jex)
        {
            throw new ArgumentException(
                "The input must be a valid JSON array of dataset definitions.",
                nameof(JsonData),
                jex);
        }
    }
}
