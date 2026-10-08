using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviders;

internal class DictionaryDataProvider : DataProviderBase<Dictionary<string, object>>
{
    public override ProviderDataStructure Process(string DatasetName, Dictionary<string, object> Data)
    {
        var result = new ProviderDataStructure();
        result.DatasetName = DatasetName;
        if (Data.TryGetValue("Id", out var idValue))
        {
            result.DatasetName = idValue?.ToString() ?? string.Empty;
        }
        result.Data = Data;
        return result;
    }
}
