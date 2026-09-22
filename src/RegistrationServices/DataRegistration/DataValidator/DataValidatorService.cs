using RightRule.RegistrationServices.DataRegistration.Data;

namespace RightRule.RegistrationServices.DataRegistration.DataValidator;

internal class DataValidatorService : IDataValidatorService
{
    public DataValidatorService()
    {
    }

    public bool IsValidDataIdentifier(IDataRegistrationService DataService, string DataIdentifier)
    {
        if (DataIdentifier.Contains('.')) // dotted
        {
            return IsValidDataDotted(DataService, DataIdentifier);
        }
        return IsValidDataNotDotted(DataService, DataIdentifier);
    }

    private bool IsValidDataNotDotted(IDataRegistrationService DataService, string DataIdentifier)
    {
        var ds = GetDataSet(DataService, DataIdentifier);
        if (ds == null)
        {
            return false;
        }
        var dataItemNames = DataService.GetDataItemNames(ds);
        return (dataItemNames.Contains(DataIdentifier));
    }

    private bool IsValidDataDotted(IDataRegistrationService DataService, string DataIdentifier)
    {
        var parts = DataIdentifier.Split('.');
        if (parts != null && parts.Length == 2)
        {
            return IsValidDataItem(DataService, parts[0], parts[1]);
        }
        return false;
    }

    private bool IsValidDataItem(IDataRegistrationService DataService, string DatasetName, string DataItemName)
    {
        var ds = DataService.GetDataset(DatasetName);
        if (ds == null)
        {
            return false;
        }
        var dataItemNames = DataService.GetDataItemNames(ds);
        return dataItemNames.Contains(DataItemName);
    }

    private Dataset? IsValidDataset(IDataRegistrationService DataService, string DatasetName)
    {
        return GetDataSet(DataService, DatasetName);
    }

    private Dataset? GetDataSet(IDataRegistrationService DataService, string DatasetName)
    {
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            return default;
        }
        return DataService.GetDataset(DatasetName);
    }
}
