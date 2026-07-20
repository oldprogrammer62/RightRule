namespace RightRule.Data;

internal class DataRegistrationService
{
    internal static RegisteredDataset CreateRegisteredDataObject(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        return new RegisteredDataset(DatasetName, Data);
    }
}
