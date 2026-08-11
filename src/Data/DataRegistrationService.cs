namespace RightRule.Data;

internal class DataRegistrationService
{
    internal static RegisteredDataset CreateRegisteredData(string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        return new RegisteredDataset(DataName, Data);
    }

    internal static RegisteredDataObject CreateRegisteredData(string DataName, object Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        return new RegisteredDataObject(DataName, Data);
    }
}
