using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataLoader;
using RightRule.DataServices.DataSerializer;
using RightRule.DataServices.DataValidator;
using System.ComponentModel;

namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
    private List<Dataset> _datasets;
    public List<Dataset> RegisteredDatasets => _datasets;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly ILogger<DataService> _logger;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly IDataValidatorService _dataValidator;

    private readonly IDataSerializerService _dataSerializerService;
    private readonly IDataLoaderService _dataLoaderService;
    private readonly IDataAdaptorService _dataAdaptorService;

    public DataService(IDataValidatorService DataValidator, IDataSerializerService DataSerializerService, IDataLoaderService dataLoaderService, IDataAdaptorService dataAdaptorService, ILogger<DataService> logger)
    {
        _logger = logger;

        _datasets = new List<Dataset>();
        _dataValidator = DataValidator;
        _dataSerializerService = DataSerializerService;
        _dataLoaderService = dataLoaderService;
        _dataAdaptorService = dataAdaptorService;
    }

    internal bool IsValidDataIdentifier(string DataIdentifier)
    {
        return _dataValidator.IsDataIdentifier(this, DataIdentifier);
    }

    public List<string>? GetDataItemNames(string DatasetName)
    {
        var dataset = GetRegisteredData(DatasetName);
        if (dataset == null)
        {
            return [];
        }
        return GetDataItemNames(dataset);
    }

    public List<string>? GetDataItemNames(Dataset Dataset)
    {
        var names = Dataset.GetDataItemNames();
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{CountOfNames} DataItem Names found in {DatasetName}",
                names?.Count ?? 0,
                Dataset.DatasetName);
        }
        return names;
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

    internal Dataset CreateDataSet(string DatasetName)
    {
        var dataset = new Dataset(DatasetName);
        _datasets.Add(dataset);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{DatasetName} created", DatasetName);
        }

        return dataset;
    }
}
