using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviders;

internal class DictionaryDataProvider : DataProviderBase<Dictionary<string, object>>
{
    public override ProviderDataStructure Process(string DatasetName, Dictionary<string, object> Data)
    {
        var result = new ProviderDataStructure();
        result.Id = DatasetName;
        if (Data.TryGetValue("Id", out var idValue))
        {
            result.Id = idValue?.ToString() ?? string.Empty;
        }
        result.Properties = Data;
        return result;
    }
}
