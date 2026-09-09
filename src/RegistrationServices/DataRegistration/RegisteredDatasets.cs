namespace RightRule.RegistrationServices.DataRegistration;

internal class RegisteredDatasets : IRegisteredDatasets
{
    private readonly List<RegisteredDataset> _data;
    public List<RegisteredDataset> Datasets => _data;

    public int Count => _data.Count;

    public RegisteredDataset? this[int index] => index >= 0 && index < _data.Count ? _data[index] : null;

    public HashSet<string> DatasetNames => new HashSet<string>(Datasets.Select(ds => ds.Name).Distinct(StringComparer.OrdinalIgnoreCase));

    public RegisteredDatasets()
    {
        _data = new List<RegisteredDataset>();
    }

    public DataRegistrationResult AddData(RegisteredDataset data)
    {
        if (data == null || string.IsNullOrEmpty(data.Name))
        {
            return DataRegistrationResult.FailureResult(string.Empty, DataTypes.DATASET, "Data is null or Data Name not specified");
        }
        var existingData = GetData(data.Name);
        if (existingData == null)
        {
            _data.Add(data);
            return DataRegistrationResult.SuccessResult(data.Name, DataTypes.DATASET, $"Data: {data.Name} registered");
        }
        return DataRegistrationResult.FailureResult(data.Name, DataTypes.DATASET, $"Data: {data.Name} already registered");
    }

    public bool RemoveData()
    {
        _data.Clear();
        return true;
    }

    public bool RemoveData(string name)
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

    public RegisteredDataset? GetData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        return _data.FirstOrDefault(d => d.Name.Equals(name, StringComparison.Ordinal));
    }
}
