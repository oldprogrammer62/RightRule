using RightRule.RegistrationServices.DataRegistration.Data;

namespace RightRule.RegistrationServices.DataRegistration.DataValidator;

internal class DataValidator : IDataValidator
{
    //private readonly IDataService _dataService;

    public DataValidator()
    {
        //_dataService = dataService;
    }

    public bool IsValidData(IDataService dataService, string name)
    {
        if (name.Contains('.')) // dotted
        {
            return IsValidDataDotted(dataService, name);
        }
        return IsValidDataNotDotted(dataService, name);
    }

    private bool IsValidDataNotDotted(IDataService dataService, string name)
    {
        var ds = GetDataSet(dataService, name);
        if (ds == null)
        {
            return false;
        }
        var dataItemNames = dataService.GetDataItemNames(ds);
        return (dataItemNames.Contains(name));
    }

    private bool IsValidDataDotted(IDataService dataService, string name)
    {
        var parts = name.Split('.');
        if (parts != null && parts.Length == 2)
        {
            return IsValidDataItem(dataService, parts[0], parts[1]);
        }
        return false;
    }

    private bool IsValidDataItem(IDataService dataService, string dataSetName, string dataName)
    {
        var ds = dataService.GetDataset(dataSetName);
        if (ds == null)
        {
            return false;
        }
        var dataItemNames = dataService.GetDataItemNames(ds);
        return dataItemNames.Contains(dataName);
    }

    private Dataset? IsValidDataset(IDataService dataService, string name)
    {
        return GetDataSet(dataService, name);
    }

    private Dataset? GetDataSet(IDataService dataService, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return default;
        }
        return dataService.GetDataset(name);
    }
}
