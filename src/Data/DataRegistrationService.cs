namespace RightRule.Data;

internal class DataRegistrationService
{
    internal static RegisteredDataset CreateRegisteredDataObject(string datasetName, Dictionary<string, object> data)
    {
        if (string.IsNullOrWhiteSpace(datasetName))
        {
            throw new ArgumentException("Dataset name cannot be null or empty.", nameof(datasetName));
        }
        return new RegisteredDataset(datasetName, data);
    }

    internal static RegisteredDataset CreateRegisteredDataObject<T>(string datasetName, T data)
    {
        var convertedData = DataConverters.DataConverter.ConvertToDictionary(data);
        return new RegisteredDataset(datasetName, convertedData);
    }
}
