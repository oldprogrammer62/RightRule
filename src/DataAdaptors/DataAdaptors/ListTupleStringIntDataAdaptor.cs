using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptors;

internal sealed class ListTupleStringIntDataAdaptor : DataAdaptorBase<List<(string Name, int Value)>>
{
    public override string AdaptorName { get; init; }

    public ListTupleStringIntDataAdaptor()
    {
        AdaptorName = "List<(string,int)> DataAdaptor";
    }

    public override AdaptorDataStructure Process(string DatasetName, List<(string Name, int Value)> Data)
    {
        var result = new AdaptorDataStructure();
        result.DatasetName = DatasetName;
        foreach (var (Name, Value) in Data)
        {
            result.Data.Add(Name, Value);
        }
        return result;
    }
}
