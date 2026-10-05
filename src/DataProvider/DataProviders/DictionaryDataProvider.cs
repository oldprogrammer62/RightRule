using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

internal class DictionaryDataProvider : DataProviderBase<Dictionary<string, object>>
{
    public override InternalDataStructure Process(string DS_Name, Dictionary<string, object> input)
    {
        var result = new InternalDataStructure();
        result.Id = DS_Name;
        if (input.TryGetValue("Id", out var idValue))
        {
            result.Id = idValue?.ToString() ?? string.Empty;
        }
        result.Properties = input;
        return result;
    }
}
