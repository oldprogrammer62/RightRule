using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

internal class IntegerDataProvider : DataProviderBase<int>
{
    public override InternalDataStructure Process(string DS_Name, int input)
    {
        var result = new InternalDataStructure();
        result.Id = DS_Name;
        result.Properties.Add(DS_Name, input);
        return result;
    }
}
