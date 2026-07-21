namespace RightRule.Data;

public class RegisteredDataset
{
    public string Name { get; init; }
    public Dictionary<string, object> Data { get; init; }

    public RegisteredDataset(string DatasetName, Dictionary<string, object> Dataset)
    {
        Name = DatasetName;
        Data = Dataset;
    }
}
