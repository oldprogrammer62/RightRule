using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptors;

internal sealed class ObjectDataAdaptor : DataAdaptorBase<object>
{
    public override string AdaptorName { get; init; }

    public ObjectDataAdaptor()
    {
        AdaptorName = "ObjectDataAdaptor";
    }

    public override AdaptorDataStructure Process(string DatasetName, object Data)
    {
        var result = new AdaptorDataStructure();
        result.DatasetName = DatasetName;
        result.Data.Add(DatasetName, Data);
        return result;
    }
}
