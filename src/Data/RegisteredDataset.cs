namespace RightRule.Data;

public class RegisteredDataset
{
    internal string Name { get; set; }
    internal Dictionary<string, object> Data { get; set; }

    public RegisteredDataset(string DatasetName, Dictionary<string, object> Dataset)
    {
        Name = DatasetName;
        Data = Dataset;
    }
}
