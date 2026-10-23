namespace RightRule.System.Common.DataAdaptor;

public sealed class AdaptorDataStructure
{
    public string DatasetName { get; set; } = string.Empty;

    public Dictionary<string, object> Data { get; set; } = new();
}
