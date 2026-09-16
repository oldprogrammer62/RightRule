namespace RightRule.RegistrationServices.DataRegistration;

internal class RegisteredDataObjects : xIRegisteredDataObjects
{
    private readonly List<RegisteredDataObject> _data;
    public List<RegisteredDataObject> Objects => _data;

    public int Count => _data.Count;

    public RegisteredDataObject? this[int index] => index >= 0 && index < _data.Count ? _data[index] : null;

    public HashSet<string> ObjectNames => new HashSet<string>(Objects.Select(ob => ob.Name).Distinct(StringComparer.OrdinalIgnoreCase));

    public RegisteredDataObjects()
    {
        _data = new List<RegisteredDataObject>();
    }

    public DataRegistrationResult AddData(RegisteredDataObject data)
    {
        if (data == null || string.IsNullOrEmpty(data.Name))
        {
            return DataRegistrationResult.FailureResult(string.Empty, DataTypes.DATAOBJECT, "Data is null or Data Name not specified");
        }
        var existingData = GetData(data.Name);
        if (existingData == null)
        {
            _data.Add(data);
            return DataRegistrationResult.SuccessResult(data.Name, DataTypes.DATAOBJECT, $"{data.Name} registered");
        }
        return DataRegistrationResult.FailureResult(data.Name, DataTypes.DATAOBJECT, $"{data.Name} already registered");
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

        var dataObject = GetData(name);
        if (dataObject != null)
        {
            _data.Remove(dataObject);
        }
        return true;
    }

    public RegisteredDataObject? GetData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        return _data.FirstOrDefault(d => d.Name.Equals(name, StringComparison.Ordinal));
    }
}
