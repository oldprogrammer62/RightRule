using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DataRegistration.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public List<Dataset>? GetData()
    {
        return DataService.GetDatasets();
    }

    public Dataset? GetData(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        return DataService.GetDataset(DataName);
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
                Status = RegistrationServices.RuleRegistration.RegistrationStatus.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }
        return new DataRegistrationResult()
        {
            Status = RegistrationServices.RuleRegistration.RegistrationStatus.SUCCESS,
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
                Status = RegistrationServices.RuleRegistration.RegistrationStatus.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }

        return new DataRegistrationResult()
        {
            Status = RegistrationServices.RuleRegistration.RegistrationStatus.SUCCESS,
            DataType = DataTypes.DATASET,
            Name = DatasetName
        };
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue)
    {
        // Add data to new Data Service
        var added = DataService.AddData(DatasetName, DataItemName, DataItemValue);
        if (added == null)
        {
            return new DataRegistrationResult()
            {
                Status = RegistrationServices.RuleRegistration.RegistrationStatus.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }

        return new DataRegistrationResult()
        {
            Status = RegistrationServices.RuleRegistration.RegistrationStatus.SUCCESS,
            DataType = DataTypes.DATASET,
            Name = DatasetName
        };
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
        DataService.DeregisterData(DatasetName);
    }

    public bool DeRegisterData(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        DeRegisterNewData(DataName);
        return true;
    }
}
