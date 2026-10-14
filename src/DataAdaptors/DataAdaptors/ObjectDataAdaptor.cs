using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptors;

internal class ObjectDataAdaptor : DataAdaptorBase<object>
{
    public override AdaptorDataStructure Process(string DatasetName, object Data)
    {
        var result = new AdaptorDataStructure();
        result.DatasetName = DatasetName;
        result.Data.Add(DatasetName, Data);
        return result;
    }
}
