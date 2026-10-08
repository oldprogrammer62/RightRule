using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviders;

internal class StringDataProvider : DataProviderBase<string>
{
    public override ProviderDataStructure Process(string DatasetName, string Data)
    {
        var result = new ProviderDataStructure();
        result.DatasetName = DatasetName;
        result.Data.Add(DatasetName, Data);
        return result;
    }
}
