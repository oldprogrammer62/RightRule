namespace RightRule.RegistrationServices.DataRegistration;

public class newAllRegisteredData
{
    public List<newRegisteredData> RegisteredData { get; set; }

    public newAllRegisteredData()
    {
        RegisteredData = new List<newRegisteredData>();
    }

    public void Clear()
    {
        if (RegisteredData == null)
        {
            return;
        }
        RegisteredData = new List<newRegisteredData>();
    }

    public void DeregisterData(string DatasetName)
    {
        if (RegisteredData == null)
        {
            return;
        }
        RegisteredData.Remove(RegisteredData.First(x => x.DataName == DatasetName));
    }

    public bool AddData(string DatasetName, Dictionary<string, object> DataItems)
    {
        newRegisteredData rd;
        var found = GetRegisteredData(DatasetName);
        if (found != null)
        {
            found.AddDataItem(DatasetName, DataItems);
            return true;
        }
        rd = new newRegisteredData();
        rd.AddDataItem(DatasetName, DataItems);
        RegisteredData.Add(rd);
        return true;
    }

    public bool AddData(string DatasetName, ValueTuple<string, object> DataItem)
    {
        newRegisteredData rd;
        var found = GetRegisteredData(DatasetName);
        if (found != null)
        {
            found.AddDataItem(DatasetName, DataItem.Item1, DataItem.Item2);
            return true;
        }
        rd = new newRegisteredData();
        rd.AddDataItem(DatasetName, DataItem.Item1, DataItem.Item2);
        RegisteredData.Add(rd);
        return true;
    }

    public bool AddData(string DatasetName, string DataItemName, object DataItemValue)
    {
        newRegisteredData rd;
        var found = GetRegisteredData(DatasetName);
        if (found != null)
        {
            found.AddDataItem(DatasetName, DataItemName, DataItemValue);
            return true;
        }
        rd = new newRegisteredData();
        rd.AddDataItem(DatasetName, DataItemName, DataItemValue);
        RegisteredData.Add(rd);
        return true;
    }

    public List<newRegisteredData> GetRegisteredData()
    {
        return RegisteredData;
    }

    public newRegisteredData? GetRegisteredData(string DataName)
    {
        return RegisteredData.FirstOrDefault(d => d.DataName.Equals(DataName, StringComparison.Ordinal));
    }
}

public class newRegisteredData
{
    public string DataName { get; private set; }
    public newDataItems Data {  get; set; }

    public newRegisteredData()
    {
        DataName = string.Empty;
        Data = new newDataItems();
    }

    public void AddDataItem(string DatasetName, Dictionary<string, object> DataItems)
    {
        this.DataName = DatasetName;
        foreach (var item in DataItems)
        {
            this.Data.AddDataItem(new newDataItem(item.Key, item.Value));
        }
    }

    public void AddDataItem(string DatasetName, object DataItem)
    {
        this.DataName = DatasetName;
        this.Data.AddDataItem(new newDataItem(DatasetName, DataItem));
    }

    public void AddDataItem(string DatasetName, string DataItemName, object DataItemValue)
    {
        this.DataName = DatasetName;
        this.Data.AddDataItem(new newDataItem(DataItemName, DataItemValue));
    }

    public object? GetDataValue(string DataName)
    {
        var dataItem = GetDataItem(DataName);
        return dataItem?.Value;
    }

    public newDataItem? GetDataItem(string DataName)
    {
        return this.Data.DataItems.FirstOrDefault(x => x.Key == DataName);
    }

    public void MergeData(RegisteredDataset dataset)
    {
        if (dataset == null)
        {
            return;
        }
        DataName = dataset.Name;

        foreach(KeyValuePair<string, object> key in dataset.Values)
        {
            Data.AddDataItem(key.Key, key.Value);
        }
    }

    public void MergeData(RegisteredDataObject dataobject)
    {
        if (dataobject == null)
        {
            return;
        }
        DataName = dataobject.Name;
        Data.AddDataItem(dataobject.Name, dataobject.Value);
    }
}

public class newDataItems
{
    public List<newDataItem> DataItems { get; init; }

    public newDataItems()
    {
        DataItems = new List<newDataItem>();
    }

    public void AddDataItem(string Key, object Value)
    {
        DataItems.Add(new newDataItem(Key, Value));
    }

    public void AddDataItem(newDataItem item)
    {
        DataItems.Add(item);
    }
}

public class newDataItem
{
    public string Key { get; init; }
    public object Value { get; init; }

    public newDataItem(string key, object value)
    {
        Key = key;
        Value = value;
    }
}
