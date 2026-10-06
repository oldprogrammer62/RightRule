using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

internal class ObjectDataProvider : DataProviderBase<object>
{
    public override InternalDataStructure Process(string DatasetName, object Data)
    {
        var result = new InternalDataStructure();
        result.Id = DatasetName;
        result.Properties.Add(DatasetName, Data);
        return result;
    }
}
