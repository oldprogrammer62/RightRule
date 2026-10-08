using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviders;

internal class ObjectDataProvider : DataProviderBase<object>
{
    public override ProviderDataStructure Process(string DatasetName, object Data)
    {
        var result = new ProviderDataStructure();
        result.Id = DatasetName;
        result.Properties.Add(DatasetName, Data);
        return result;
    }
}
