using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviders;

internal class StringDataProvider : DataProviderBase<string>
{
    public override ProviderDataStructure Process(string DatasetName, string Data)
    {
        var result = new ProviderDataStructure();
        result.Id = DatasetName;
        result.Properties.Add(DatasetName, Data);
        return result;
    }
}
