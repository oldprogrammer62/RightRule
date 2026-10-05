namespace RightRule.DataProviders;

public class InternalDataStructure
{
    public string Id { get; set; } = string.Empty;

    public Dictionary<string, object> Properties { get; set; } = new();
}
