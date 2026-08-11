namespace RightRule.Data;

internal class DataRegistrationService
{
    internal static RegisteredDataset CreateRegisteredData(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        return new RegisteredDataset(DatasetName, Data);
    }

    internal static RegisteredDataObject CreateRegisteredData(string DatasetName, object Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        return new RegisteredDataObject(DatasetName, Data);
    }
}
