using Microsoft.Extensions.Logging;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.RuleServices.RuleRegistration;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public List<Dataset>? GetDataset()
    {
        return DataService.GetDatasets();
    }

    public Dataset? GetDataset(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        return DataService.GetDataset(DatasetName);
    }

    public DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems)
    {
        ArgumentNullException.ThrowIfNull(DataItems, nameof(DataItems));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        // Add data to new Data Service
        var added = DataService.AddData(DatasetName, DataItems);
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
        var added = DataService.AddData(DatasetName, DataItem);
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

    public List<DataRegistrationResult> RegisterData(string DatasetName, List<ValueTuple<string, object>> DataItems)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        var results = new List<DataRegistrationResult>();

        foreach (var dataItem in DataItems)
        {
            // Add data to new Data Service
            var added = DataService.AddData(DatasetName, dataItem);
            if (added == null)
            {
                results.Add(new DataRegistrationResult()
                {
                    Status = RegistrationStatusEnum.FAILURE,
                    DataType = DataTypes.DATASET,
                    Name = DatasetName
                });
            }
            results.Add(new DataRegistrationResult()
            {
                Status = RegistrationStatusEnum.SUCCESS,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            });
        }
        return results;
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue)
    {
        logger.LogInformation("RightRuleService was here");

        // Add data to new Data Service
        var added = DataService.AddData(DatasetName, DataItemName, DataItemValue);
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
        DataService.Clear();
    }

    private void DeRegisterNewData(string DatasetName)
    {
        DataService.DeregisterDataset(DatasetName);
    }
}
