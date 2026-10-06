using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

internal class TupleStringIntDataProvider : DataProviderBase<(string Name, int Value)>
{
    public override InternalDataStructure Process(string DatasetName, (string Name, int Value) Data)
    {
        var result = new InternalDataStructure();
        result.Id = DatasetName;
        result.Properties.Add(Data.Name, Data.Value);
        return result;
    }
}
