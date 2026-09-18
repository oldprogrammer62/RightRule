using RightRule.RegistrationServices.DataRegistration.Data;

namespace RightRule.RegistrationServices.DataRegistration;

internal interface IDataService
{
    List<string> GetDataSetNames();
    List<string> GetDataItemNames();
    List<string> GetDataItemNames(Dataset dataset);

    Dataset AddData(string DatasetName, (string, object) DataItem);
    Dataset AddData(string DatasetName, Dictionary<string, object> DataItems);
    Dataset AddData(string DatasetName, List<(string, object)> DataItems);
    Dataset AddData(string DatasetName, object DataItemValue);
    Dataset AddData(string DatasetName, string DataItemName, object DataItemValue);
    void Clear();
    Dataset CreateDataSet(string DatasetName);
    void DeregisterData(string DatasetName);
    Dataset? GetDataset(string DataName);
    List<Dataset>? GetDatasets();

    DataItem? GetDataItem(string DatasetName, string DataName);
    object? GetDataValue(string DatasetName, string DataName);

    DataItem? GetDataItem(string DataName);
    object? GetDataValue(string DataName);

    bool IsValidData(string name);
}
