using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

internal class StringDataProvider : DataProviderBase<string>
{
    public override InternalDataStructure Process(string DS_Name, string input)
    {
        var result = new InternalDataStructure();
        result.Id = DS_Name;
        result.Properties.Add(DS_Name, input);
        return result;
    }
}
