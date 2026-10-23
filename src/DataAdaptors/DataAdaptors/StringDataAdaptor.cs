using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptors;

internal sealed class StringDataAdaptor : DataAdaptorBase<string>
{
    public override string AdaptorName { get; init; }

    public StringDataAdaptor()
    {
        AdaptorName = "StringDataAdaptor";
    }

    public override AdaptorDataStructure Process(string DatasetName, string Data)
    {
        var result = new AdaptorDataStructure();
        result.DatasetName = DatasetName;
        result.Data.Add(DatasetName, Data);
        return result;
    }
}
