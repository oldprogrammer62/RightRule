namespace RightRule.Data;

public class RegisteredDatasets
{
    public List<RegisteredDataset> _datasets;

    public RegisteredDatasets()
    {
        _datasets = new List<RegisteredDataset>();
    }

    public int Count => _datasets.Count;
    public RegisteredDataset? this[int index] => index >= 0 && index < _datasets.Count ? _datasets[index] : null;

    public bool AddDataset(RegisteredDataset dataset)
    {
        if (dataset == null || string.IsNullOrEmpty(dataset.Name))
        {
            return false;
        }

        _datasets.Add(dataset);
        return true;
    }

    public bool RemoveDatasets()
    {
        _datasets.Clear();
        return true;
    }

    public bool RemoveDataset(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        var dataset = GetDataset(name);
        if (dataset != null)
        {
            _datasets.Remove(dataset);
        }
        return true;
    }

    private RegisteredDataset? GetDataset(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        return _datasets.FirstOrDefault(d => d.Name.Equals(name, StringComparison.Ordinal));
    }
}
