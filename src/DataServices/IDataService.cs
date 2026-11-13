using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;

namespace RightRule.DataServices;

public interface IDataService
{
    DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull;
    
    List<Dataset> RegisteredDatasets { get; }

    bool DeRegisterData();
    
    Dataset CreateDataSet(string DatasetName);
    List<string> GetDataSetNames();
    List<Dataset> GetRegisteredData();
    Dataset? GetRegisteredData(string DatasetName);

    bool DeRegisterData(string DatasetName);

    Dataset AddData(string DatasetName, (string, object) DataItem);
    Dataset AddData(string DatasetName, List<(string, object)> DataItems);
    Dataset AddData(string DatasetName, Dictionary<string, object> DataItems);
    Dataset AddData(string DatasetName, string DataItemName, object DataItemValue);

    DataItem? GetDataItem(string DatasetName, string DataItemName);
    List<string>? GetDataItemNames(string DatasetName);
    List<string>? GetDataItemNames(Dataset Dataset);
    object? GetDataItemValue(string DatasetName, string DataItemName);

    bool IsValidDataIdentifier(string name);

    string SerializeData();
    string SerializeData(string DatasetName);

    List<DataRegistrationResult> LoadData(string JsonData);
}
