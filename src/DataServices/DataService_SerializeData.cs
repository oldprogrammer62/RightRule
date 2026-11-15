namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
    public string SerializeData()
    {
        var datasets = this._datasets;
        if (datasets == null || datasets.Count == 0)
        {
            return string.Empty;
        }
        return _dataSerializerService.SerializeData(datasets);
    }

    public string SerializeData(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        var dataset = GetRegisteredData(DatasetName);
        if (dataset == null)
        {
            return string.Empty;
        }
        // Must wrap the dataset in a list to serialize it correctly
        return _dataSerializerService.SerializeData([dataset]);
    }
}
