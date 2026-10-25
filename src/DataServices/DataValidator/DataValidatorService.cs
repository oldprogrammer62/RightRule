using RightRule.DataServices.Data;

namespace RightRule.DataServices.DataValidator;

internal sealed class DataValidatorService : IDataValidatorService
{
    public DataValidatorService()
    {
    }

    public bool IsDataIdentifier(IDataService DataService, string DataIdentifier)
    {
        if (DataIdentifier.Contains('.')) // dotted
        {
            return DataDotted(DataService, DataIdentifier);
        }
        return DataNotDotted(DataService, DataIdentifier);
    }

    private bool DataNotDotted(IDataService DataService, string DataIdentifier)
    {
        var ds = GetDataSet(DataService, DataIdentifier);
        if (ds == null)
        {
            return false;
        }
        var dataItemNames = DataService.GetDataItemNames(ds);
        if (dataItemNames is null)
        {
            return false;
        }
        return (dataItemNames.Contains(DataIdentifier));
    }

    private bool DataDotted(IDataService DataService, string DataIdentifier)
    {
        var parts = DataIdentifier.Split('.');
        if (parts != null && parts.Length == 2)
        {
            return ValidDataItem(DataService, parts[0], parts[1]);
        }
        return false;
    }

    private bool ValidDataItem(IDataService DataService, string DatasetName, string DataItemName)
    {
        var ds = DataService.GetDataset(DatasetName);
        if (ds == null)
        {
            return false;
        }
        var dataItemNames = DataService.GetDataItemNames(ds);
        if (dataItemNames is null)
        {
            return false;
        }
        return dataItemNames.Contains(DataItemName);
    }

    private Dataset? ValidDataset(IDataService DataService, string DatasetName)
    {
        return GetDataSet(DataService, DatasetName);
    }

    private static Dataset? GetDataSet(IDataService DataService, string DatasetName)
    {
        if (string.IsNullOrWhiteSpace(DatasetName))
        {
            return default;
        }
        return DataService.GetDataset(DatasetName);
    }
}
