namespace RightRule.RegistrationServices.DataRegistration
{
    public interface InewGetDataService
    {
        void Clear();
        void DeregisterData(string DatasetName);

        newAllRegisteredData _registeredData { get; init; }

        bool newAddData(string DatasetName, Dictionary<string, object> DataItems);
        bool newAddData(string DatasetName, ValueTuple<string, object> DataItem);
        bool newAddData(string DatasetName, string DataItemName, object DataItemValue);

        List<newRegisteredData>? newGetData();
        newRegisteredData? newGetData(string DataName);

        object? newGetDataValue(string LeftSide, string RightSide);
    }
}