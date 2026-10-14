using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptors;

internal class StringDataAdaptor : DataAdaptorBase<string>
{
    public override AdaptorDataStructure Process(string DatasetName, string Data)
    {
        var result = new AdaptorDataStructure();
        result.DatasetName = DatasetName;
        result.Data.Add(DatasetName, Data);
        return result;
    }
}
