using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

internal class StringDataProvider : DataProviderBase<string>
{
    public override InternalDataStructure Process(string DatasetName, string Data)
    {
        var result = new InternalDataStructure();
        result.Id = DatasetName;
        result.Properties.Add(DatasetName, Data);
        return result;
    }
}
