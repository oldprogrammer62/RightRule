namespace RightRule.Data;

internal class DataRegistrationService
{
    /// <summary>
    /// Creates a registered dataset object with the specified name and data. The data is provided as a dictionary of key-value pairs.
    /// </summary>
    /// <param name="DatasetName">The name of the dataset.</param>
    /// <param name="Data">The data to register.</param>
    /// <returns>The registered dataset.</returns>
    /// <exception cref="ArgumentException"></exception>
    internal static RegisteredDataset CreateRegisteredDataObject(string DatasetName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrEmpty(DatasetName, nameof(DatasetName));
        ArgumentNullException.ThrowIfNull(Data);
        return new RegisteredDataset(DatasetName, Data);
    }
}
