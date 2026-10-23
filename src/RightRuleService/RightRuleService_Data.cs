using Microsoft.Extensions.Logging;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.Models;
using RightRule.System.Common.DataAdaptor;

namespace RightRule.RightRuleService;

internal sealed partial class RightRuleService : IRightRuleService
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

    public DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull
    {
        if (DataAdaptorService == null)
        {
            return new DataRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }

        AdaptorDataStructure dataResult = DataAdaptorService.ProcessIncomingData<T>(DatasetName, Data);
        if (dataResult == null)
        {
            return new DataRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }
        var dataRegistrationResult = RegisterIncomingData(DatasetName, dataResult.Data);

        if (dataRegistrationResult.Status == RightRuleStatusEnum.SUCCESS)
        {
            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation(message: "Data Registration Success for Datset: {datasetName}", DatasetName);
            }
        }
        return dataRegistrationResult;
    }

    private DataRegistrationResult RegisterIncomingData(string DatasetName, Dictionary<string, object> DataItems)
    {
        ArgumentNullException.ThrowIfNull(DataItems, nameof(DataItems));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        // Add data to new Data Service
        var added = DataService.AddData(DatasetName, DataItems);
        if (added == null)
        {
            return new DataRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }
        return new DataRegistrationResult()
        {
            Status = RightRuleStatusEnum.SUCCESS,
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
