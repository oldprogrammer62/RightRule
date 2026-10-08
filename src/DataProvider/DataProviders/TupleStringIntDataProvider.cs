using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviders;

internal class TupleStringIntDataProvider : DataProviderBase<(string Name, int Value)>
{
    public override ProviderDataStructure Process(string DatasetName, (string Name, int Value) Data)
    {
        var result = new ProviderDataStructure();
        result.Id = DatasetName;
        result.Properties.Add(Data.Name, Data.Value);
        return result;
    }
}
