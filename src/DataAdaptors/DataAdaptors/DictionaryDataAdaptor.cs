using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptors;

internal sealed class DictionaryDataAdaptor : DataAdaptorBase<Dictionary<string, object>>
{
    public override string AdaptorName { get; init; }

    public DictionaryDataAdaptor()
    {
        AdaptorName = "DictionaryDataAdaptor";
    }

    public override AdaptorDataStructure Process(string DatasetName, Dictionary<string, object> Data)
    {
        var result = new AdaptorDataStructure();
        result.DatasetName = DatasetName;
        if (Data.TryGetValue("Id", out var idValue))
        {
            result.DatasetName = idValue?.ToString() ?? string.Empty;
        }
        result.Data = Data;
        return result;
    }
}
