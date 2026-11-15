using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;

namespace RightRule.DataServices;

public interface IDataService
{
    DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull;
    
    List<Dataset> RegisteredDatasets { get; }
    
    List<string> GetDataSetNames();

    List<Dataset> GetRegisteredData();
    Dataset? GetRegisteredData(string DatasetName);

    bool DeRegisterData();
    bool DeRegisterData(string DatasetName);

    DataItem? GetDataItem(string DatasetName, string DataItemName);
    List<string>? GetDataItemNames(string DatasetName);
    List<string>? GetDataItemNames(Dataset Dataset);
    object? GetDataItemValue(string DatasetName, string DataItemName);

    string SerializeData();
    string SerializeData(string DatasetName);

    List<DataRegistrationResult> LoadData(string JsonData);
}
