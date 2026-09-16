using RightRule.Data;

namespace RightRule.RegistrationServices.DataRegistration
{
    public interface InewGetDataService
    {
        void Clear();
        void DeregisterData(string DatasetName);

        DataService _registeredData { get; init; }

        bool newAddData(string DatasetName, Dictionary<string, object> DataItems);
        bool newAddData(string DatasetName, ValueTuple<string, object> DataItem);
        bool newAddData(string DatasetName, string DataItemName, object DataItemValue);

        List<Dataset>? GetData();
        Dataset? GetData(string DataName);

        object? newGetDataValue(string LeftSide, string RightSide);
    }
}