namespace RightRule.Data;

public class RegisteredDatasets
{
    private readonly List<RegisteredDataset> _data;
    public List<RegisteredDataset> Datasets => _data;

    public int Count => _data.Count;

    public RegisteredDataset? this[int index] => index >= 0 && index < _data.Count ? _data[index] : null;

    internal RegisteredDatasets()
    {
        _data = new List<RegisteredDataset>();
    }

    internal bool AddData(RegisteredDataset data)
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

    internal RegisteredDataset? GetData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        return _data.FirstOrDefault(d => d.Name.Equals(name, StringComparison.Ordinal));
    }
}
