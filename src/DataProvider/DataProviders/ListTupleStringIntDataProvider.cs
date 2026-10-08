using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviders;

internal class ListTupleStringIntDataProvider : DataProviderBase<List<(string Name, int Value)>>
{
    public override ProviderDataStructure Process(string DatasetName, List<(string Name, int Value)> Data)
    {
        var result = new ProviderDataStructure();
        result.DatasetName = DatasetName;
        foreach (var (Name, Value) in Data)
        {
            result.Data.Add(Name, Value);
        }
        return result;
    }
}
