using System.Linq;

namespace RightRule.Data;

public class RegisteredDataObjects
{
    private readonly List<RegisteredDataObject> _data;
    public List<RegisteredDataObject> Objects => _data;

    public int Count => _data.Count;

    public RegisteredDataObject? this[int index] => index >= 0 && index < _data.Count ? _data[index] : null;

    public HashSet<string> ObjectNames => new HashSet<string>(Objects.Select(ob => ob.DataName).Distinct(StringComparer.OrdinalIgnoreCase));

    internal RegisteredDataObjects()
    {
        _data = new List<RegisteredDataObject>();
    }

    internal bool AddData(RegisteredDataObject data)
    {
        if (data == null || string.IsNullOrEmpty(data.DataName))
        {
            return false;
        }
        var existingData = GetData(data.DataName);
        if (existingData == null)
        {
            _data.Add(data);
            return true;
        }
        return false;
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

        var dataObject = GetData(name);
        if (dataObject != null)
        {
            _data.Remove(dataObject);
        }
        return true;
    }

    internal RegisteredDataObject? GetData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        return _data.FirstOrDefault(d => d.DataName.Equals(name, StringComparison.Ordinal));
    }
}
