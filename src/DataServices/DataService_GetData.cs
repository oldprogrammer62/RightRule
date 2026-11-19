using Microsoft.Extensions.Logging;
using RightRule.DataServices.Data;

namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
    public DataItem? GetDataItem(string DatasetName, string DataItemName)
    {
        var ds = GetRegisteredData(DatasetName);
        if (ds != null)
        {
            return ds.GetDataItem(DataItemName);
        }
        return null;
    }

    public object? GetDataItemValue(string DatasetName, string DataItemName)
    {
        var ds = GetRegisteredData(DatasetName);
        if (ds != null)
        {
            var di = ds.GetDataItem(DataItemName);
            if (di != null)
            {
                return di.Value;
            }
        }
        return null;
    }

    public List<string> GetDataItemNames(string DatasetName)
    {
        var dataset = GetRegisteredData(DatasetName);
        if (dataset == null)
        {
            return [];
        }
        return GetDataItemNames(dataset);
    }

    public List<string> GetDataItemNames(Dataset Dataset)
    {
        var names = Dataset.GetDataItemNames();
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{CountOfNames} DataItem Names found in {DatasetName}",
                names?.Count ?? 0,
                Dataset.DatasetName);
        }
        return names ?? [];
    }

    public List<string> GetDataSetNames()
    {
        List<string> names = new List<string>();
        foreach (Dataset dataset in _datasets)
        {
            names.AddRange(dataset.DatasetName);
        }
        return names;
    }

    public List<Dataset> GetRegisteredData()
    {
        return _datasets ?? [];
    }

    public Dataset? GetRegisteredData(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        return _datasets.FirstOrDefault(d => d.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase));
    }
}
