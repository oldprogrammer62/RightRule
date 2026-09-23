using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.RuleServices.RuleRegistration;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public List<Dataset>? GetDataset()
    {
        return DataRegistrationService.GetDatasets();
    }

    public Dataset? GetDataset(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        return DataRegistrationService.GetDataset(DatasetName);
    }

    public DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems)
    {
        ArgumentNullException.ThrowIfNull(DataItems, nameof(DataItems));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        // Add data to new Data Service
        var added = DataRegistrationService.AddData(DatasetName, DataItems);
        if (added == null)
        {
            return new DataRegistrationResult()
            {
                Status = RegistrationStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }
        return new DataRegistrationResult()
        {
            Status = RegistrationStatusEnum.SUCCESS,
            DataType = DataTypes.DATASET,
            Name = DatasetName
        };
    }

    public DataRegistrationResult RegisterData(string DatasetName, ValueTuple<string, object> DataItem)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        // Add data to new Data Service
        var added = DataRegistrationService.AddData(DatasetName, DataItem);
        if (added == null)
        {
            return new DataRegistrationResult()
            {
                Status = RegistrationStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }

        return new DataRegistrationResult()
        {
            Status = RegistrationStatusEnum.SUCCESS,
            DataType = DataTypes.DATASET,
            Name = DatasetName
        };
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue)
    {
        // Add data to new Data Service
        var added = DataRegistrationService.AddData(DatasetName, DataItemName, DataItemValue);
        if (added == null)
        {
            return new DataRegistrationResult()
            {
                Status = RegistrationStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }

        return new DataRegistrationResult()
        {
            Status = RegistrationStatusEnum.SUCCESS,
            DataType = DataTypes.DATASET,
            Name = DatasetName
        };
    }

    public bool DeRegisterData(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        DeRegisterNewData(DatasetName);
        return true;
    }

    public bool DeRegisterData()
    {
        DeRegisterNewData();
        return true;
    }

    private void DeRegisterNewData()
    {
        DataRegistrationService.Clear();
    }

    private void DeRegisterNewData(string DatasetName)
    {
        DataRegistrationService.DeregisterDataset(DatasetName);
    }
}
