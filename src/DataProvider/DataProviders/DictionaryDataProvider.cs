using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

internal class DictionaryDataProvider : DataProviderBase<Dictionary<string, object>>
{
    public override InternalDataStructure Process(string DatasetName, Dictionary<string, object> Data)
    {
        var result = new InternalDataStructure();
        result.Id = DatasetName;
        if (Data.TryGetValue("Id", out var idValue))
        {
            result.Id = idValue?.ToString() ?? string.Empty;
        }
        result.Properties = Data;
        return result;
    }
}
