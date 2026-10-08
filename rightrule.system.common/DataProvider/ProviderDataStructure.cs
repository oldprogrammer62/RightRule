namespace RightRule.System.Common.DataProvider;

public class ProviderDataStructure
{
    public string Id { get; set; } = string.Empty;

    public Dictionary<string, object> Properties { get; set; } = new();
}
