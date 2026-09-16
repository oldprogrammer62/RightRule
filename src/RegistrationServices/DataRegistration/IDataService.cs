using RightRule.RegistrationServices.DataRegistration.Data;
using System.Runtime.InteropServices.ObjectiveC;

namespace RightRule.RegistrationServices.DataRegistration
{
    public interface IDataService
    {
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
    }
}