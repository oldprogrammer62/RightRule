namespace RightRule.Data;

public class RegisteredDataset
{
    public string Name { get; init; }
    public Dictionary<string, object> Value { get; init; }

    internal RegisteredDataset(string DataName, Dictionary<string, object> Dataset)
    {
        Name = DataName;
        Value = Dataset;
    }
}
