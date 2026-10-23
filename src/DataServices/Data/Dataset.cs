namespace RightRule.DataServices.Data;

public sealed class Dataset
{
    public string DatasetName { get; init; }

    public List<DataItem> DataItems { get; init; }

    public List<string> DataItemNames => GetDataItemNames();

    public Dataset(string DatasetName)
    {
        this.DatasetName = DatasetName;
        this.DataItems = new List<DataItem>();
    }

    public Dataset(string DatasetName, string DataItemName, object DataItemValue)
        : this(DatasetName)
    {
        AddDataItem(DataItemName, DataItemValue);
    }

    public Dataset(string DatasetName, Dictionary<string, object> DataItems)
        : this(DatasetName)
    {
        AddDataItems(DataItems);
    }

    private List<string> GetDataItemNames()
    {
        return this.DataItems.Select(x => x.Key).ToList();
    }

    public void AddDataItems(Dictionary<string, object> DataItems)
    {
        foreach (var item in DataItems)
        {
            AddDataItem(item.Key, item.Value);
        }
    }

    public void AddDataItems(List<ValueTuple<string, object>> DataItems)
    {
        foreach (var item in DataItems)
        {
            AddDataItem(item.Item1, item.Item2);
        }
    }

    public void AddDataItem(string DataItemName, object DataItemValue)
    {
        this.DataItems.Add(new DataItem(DataItemName, DataItemValue));
    }

    public object? GetDataValue(string DataItemName)
    {
        var dataItem = GetDataItem(DataItemName);
        return dataItem?.Value;
    }

    public DataItem? GetDataItem(string DataItemName)
    {
        return this.DataItems.FirstOrDefault(x => x.Key.Equals(DataItemName, StringComparison.OrdinalIgnoreCase));
    }

    public List<DataItem> GetDataItems()
    {
        return this.DataItems;
    }
}
