namespace RightRule.Data;

public class RegisteredDataset
{
    internal string Name { get; set; }
    internal Dictionary<string, object> Data { get; set; }
    internal bool StaticData { get; set; } = false;

    public RegisteredDataset(string DatasetName, Dictionary<string, object> Dataset, bool StaticData = false)
    {
        Name = DatasetName;
        Data = Dataset;
        this.StaticData = StaticData;
    }
}
