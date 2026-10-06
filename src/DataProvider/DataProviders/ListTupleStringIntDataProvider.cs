using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

internal class ListTupleStringIntDataProvider : DataProviderBase<List<(string Name, int Value)>>
{
    public override InternalDataStructure Process(string DatasetName, List<(string Name, int Value)> Data)
    {
        var result = new InternalDataStructure();
        result.Id = DatasetName;
        foreach (var (Name, Value) in Data)
        {
            result.Properties.Add(Name, Value);
        }
        return result;
    }
}
