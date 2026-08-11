namespace RightRule.Data;

public class RegisteredDataObjects
{
    private readonly List<RegisteredDataObject> _data;
    public List<RegisteredDataObject> Objects => _data;

    public int Count => _data.Count;

    public RegisteredDataObject? this[int index] => index >= 0 && index < _data.Count ? _data[index] : null;

    internal RegisteredDataObjects()
    {
        _data = new List<RegisteredDataObject>();
    }

    internal bool AddData(RegisteredDataObject data)
    {
        if (data == null || string.IsNullOrEmpty(data.Name))
        {
            return false;
        }

        _data.Add(data);
        return true;
    }

    internal bool RemoveData()
    {
        _data.Clear();
        return true;
    }

    internal bool RemoveData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        var dataset = GetData(name);
        if (dataset != null)
        {
            _data.Remove(dataset);
        }
        return true;
    }

    internal RegisteredDataObject? GetData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        return _data.FirstOrDefault(d => d.Name.Equals(name, StringComparison.Ordinal));
    }
}
