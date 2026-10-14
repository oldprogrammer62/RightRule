using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptors;

internal class TupleStringIntDataAdaptor : DataAdaptorBase<(string Name, int Value)>
{
    public override AdaptorDataStructure Process(string DatasetName, (string Name, int Value) Data)
    {
        var result = new AdaptorDataStructure();
        result.DatasetName = DatasetName;
        result.Data.Add(Data.Name, Data.Value);
        return result;
    }
}
