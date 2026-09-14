using RightRule.RegistrationServices.DataRegistration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public List<newRegisteredData>? newGetData()
    {
        return NewDataService.newGetData();
    }

    public newRegisteredData? newGetData(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        return NewDataService.newGetData(DataName);
    }

    public RegisteredDataset? GetDataSet(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        if (_registeredDataSets == null)
        {
            return null;
        }
        return _registeredDataSets.GetData(DataName);
    }

    public RegisteredDataObject? GetDataObject(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        if (_registeredDataObjects == null)
        {
            return null;
        }
        return _registeredDataObjects.GetData(DataName);
    }

    public DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems)
    {
        ArgumentNullException.ThrowIfNull(DataItems, nameof(DataItems));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        // Add data to new Data Service
        var added = NewDataService.newAddData(DatasetName, DataItems);
        if (!added)
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

        //var data = DataRegistrationService.CreateRegisteredData(DataName, Data);
        //return _registeredDataSets!.AddData(data);
    }

    public DataRegistrationResult RegisterData(string DatasetName, ValueTuple<string, object> DataItem)
    {
        ArgumentNullException.ThrowIfNull(DataItem, nameof(DataItem));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        // Add data to new Data Service
        var added = NewDataService.newAddData(DatasetName, DataItem);
        if (!added)
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

        //var data = DataRegistrationService.CreateRegisteredData(DataName, VariableValue);
        //return _registeredDataObjects!.AddData(data);
    }

    public DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue)
    {
        // Add data to new Data Service
        var added = NewDataService.newAddData(DatasetName, DataItemName, DataItemValue);
        if (!added)
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
        DeregisterDataObjects();
        DergisterDatasets();
        return true;
    }

    private void DeRegisterNewData()
    {
        NewDataService.Clear();
    }

    private void DeRegisterNewData(string DatasetName)
    {
        NewDataService.DeregisterData(DatasetName);
    }

    private void DeregisterDataObjects()
    {
        if (_registeredDataObjects == null)
        {
            return;
        }
        _registeredDataObjects.RemoveData();
    }

    private void DergisterDatasets()
    {
        if (_registeredDataSets == null)
        {
            return;
        }
        _registeredDataSets.RemoveData();
    }

    public bool DeRegisterData(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        DeRegisterNewData(DataName);
        DeRegisterDataObjects(DataName);
        DeRegisterDatasets(DataName);
        return true;
    }

    private void DeRegisterDataObjects(string DataName)
    {
        if (_registeredDataObjects == null)
        {
            return;
        }
        _registeredDataObjects.RemoveData(DataName);
    }

    private void DeRegisterDatasets(string DataName)
    {
        if (_registeredDataSets == null)
        {
            return;
        }
        _registeredDataSets.RemoveData(DataName);
    }
}
