namespace RightRule.RegistrationServices.DataRegistration;

internal class DataRegistrationService
{
    internal static RegisteredDataset CreateRegisteredData(string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        return new RegisteredDataset(DataName, Data);
    }

    internal static RegisteredDataObject CreateRegisteredData(string DataName, object DataValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(DataValue, nameof(DataValue));

        return new RegisteredDataObject(DataName, DataValue);
    }
}
