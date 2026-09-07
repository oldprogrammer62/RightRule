using RightRule.DataRegistration;
using RightRule.Models.DataRegistration;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
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

    public DataRegistrationResult RegisterData(string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        var data = DataRegistrationService.CreateRegisteredData(DataName, Data);
        return _registeredDataSets!.AddData(data);
    }

    public DataRegistrationResult RegisterData(string DataName, object VariableValue)
    {
        ArgumentNullException.ThrowIfNull(VariableValue, nameof(VariableValue));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        var data = DataRegistrationService.CreateRegisteredData(DataName, VariableValue);
        return _registeredDataObjects!.AddData(data);
    }

    public bool DeRegisterData()
    {
        DeregisterDataObjects();
        DergisterDatasets();
        return true;
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
