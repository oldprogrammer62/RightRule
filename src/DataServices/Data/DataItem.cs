namespace RightRule.DataServices.Data;

public class DataItem
{
    public string Key { get; init; }
    public object Value { get; init; }

    public DataItem(string key, object value)
    {
        Key = key;
        Value = value;
    }
}
