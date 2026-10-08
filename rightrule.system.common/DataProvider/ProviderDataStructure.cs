namespace RightRule.System.Common.DataProvider;

public class ProviderDataStructure
{
    public string DatasetName { get; set; } = string.Empty;

    public Dictionary<string, object> Data { get; set; } = new();
}
