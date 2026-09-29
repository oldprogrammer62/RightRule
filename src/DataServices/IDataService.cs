using RightRule.DataServices.Data;

namespace RightRule.DataServices;

public interface IDataService
{
    void Clear();

    Dataset CreateDataSet(string DatasetName);
    List<string> GetDataSetNames();
    List<Dataset>? GetDatasets();
    Dataset? GetDataset(string DatasetName);

    void DeregisterDataset(string DatasetName);

    Dataset AddData(string DatasetName, (string, object) DataItem);
    Dataset AddData(string DatasetName, List<(string, object)> DataItems);
    Dataset AddData(string DatasetName, Dictionary<string, object> DataItems);
    Dataset AddData(string DatasetName, string DataItemName, object DataItemValue);

    DataItem? GetDataItem(string DatasetName, string DataItemName);
    List<string> GetDataItemNames(string DatasetName);
    List<string> GetDataItemNames(Dataset Dataset);
    object? GetDataItemValue(string DatasetName, string DataItemName);

    bool IsValidDataIdentifier(string name);
}
