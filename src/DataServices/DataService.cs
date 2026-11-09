using Microsoft.Extensions.Logging;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataLoader;
using RightRule.DataServices.DataRegistration;
using RightRule.DataServices.DataSerializer;
using RightRule.DataServices.DataValidator;
using RightRule.Models;
using System.ComponentModel;

namespace RightRule.DataServices;

internal sealed class DataService : IDataService
{
    private List<Dataset> _datasets;
    public List<Dataset> RegisteredDatasets => _datasets;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly ILogger<DataService> _logger;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly IDataValidatorService _dataValidator;
    private readonly IDataSerializerService _dataSerializerService;
    private readonly IDataLoaderService _dataLoaderService;

    public DataService(IDataValidatorService DataValidator, IDataSerializerService DataSerializerService, IDataLoaderService dataLoaderService, ILogger<DataService> logger)
    {
        _logger = logger;

        _datasets = new List<Dataset>();
        _dataValidator = DataValidator;
        _dataSerializerService = DataSerializerService;
        _dataLoaderService = dataLoaderService;
    }

    public void Clear()
    {
        if (_datasets == null || _datasets.Count == 0)
        {
            return;
        }
        _datasets = new List<Dataset>();

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Datasets Cleared");
        }
    }

    public bool IsValidDataIdentifier(string DataIdentifier)
    {
        return _dataValidator.IsDataIdentifier(this, DataIdentifier);
    }

    public List<string>? GetDataItemNames(string DatasetName)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            return [];
        }
        return GetDataItemNames(dataset);
    }

    public List<string>? GetDataItemNames(Dataset Dataset)
    {
        var names = Dataset.DataItemNames;
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

    public void DeregisterDataset(string DatasetName)
    {
        if (_datasets == null || _datasets.Count == 0)
        {
            return;
        }
        _datasets.Remove(_datasets.First(x => x.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase)));

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{DatasetName} deregistered", DatasetName);
        }
    }

    public Dataset CreateDataSet(string DatasetName)
    {
        var dataset = new Dataset(DatasetName);
        _datasets.Add(dataset);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{DatasetName} created", DatasetName);
        }

        return dataset;
    }

    // Add Dictionary of data
    // Will be discover as DatasetName.Identifier
    public Dataset AddData(string DatasetName, Dictionary<string, object> DataItems)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItems(DataItems);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Dictionary Data. {CountOfData} Data Items added to {DatasetName}",
                    DataItems?.Count ?? 0,
                    DatasetName);
        }

        return dataset;
    }

    // Add Tuple of data
    // Will be discover as DatasetName.Identifier
    public Dataset AddData(string DatasetName, ValueTuple<string, object> DataItem)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItem(DataItem.Item1, DataItem.Item2);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogInformation("(string,object) Data. {countOfData} Data Item added to {DatasetName}",
                    1,
                    DatasetName);
        }

        return dataset;
    }

    // Add List of Tuples of data
    // Will be discover as DatasetName.Identifier
    public Dataset AddData(string DatasetName, List<ValueTuple<string, object>> DataItems)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItems(DataItems);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogInformation("List<(string,object)> Data. {CountOfData} Data Items added to {DatasetName}",
                    DataItems?.Count ?? 0,
                    DatasetName);
        }

        return dataset;
    }

    // Add Variable by name
    // Will be discover as DatasetName.Identifier
    public Dataset AddData(string DatasetName, string DataItemName, object DataItemValue)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItem(DataItemName, DataItemValue);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogInformation("Simple Variable. {CountOfData} Data Item added to {DatasetName}",
                    1,
                    DatasetName);
        }

        return dataset;
    }

    public List<DataRegistrationResult> LoadData(string JsonData)
    {
        var dataDef = _dataLoaderService.LoadData(JsonData);
        if (dataDef == null)
        {
            return [];
        }
        return AddData(dataDef);
    }

    private List<DataRegistrationResult> AddData(List<Dataset> Datasets)
    {
        var results = new List<DataRegistrationResult>();
        foreach (var dataset in Datasets)
        {
            _datasets.Add(dataset);
            var result = new DataRegistrationResult()
            {
                Status = RightRuleStatusEnum.SUCCESS,
                DataType = DataTypes.DATASET,
                Name = dataset.DatasetName
            };
            results.Add(result);
        }
        return results;
    }

    public List<Dataset>? GetDatasets()
    {
        return _datasets;
    }

    public Dataset? GetDataset(string DatasetName)
    {
        return _datasets.FirstOrDefault(d => d.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase));
    }

    public DataItem? GetDataItem(string DatasetName, string DataItemName)
    {
        var ds = GetDataset(DatasetName);
        if (ds != null)
        {
            return ds.GetDataItem(DataItemName);
        }
        return null;
    }

    public object? GetDataItemValue(string DatasetName, string DataItemName)
    {
        var ds = GetDataset(DatasetName);
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

    public string DeserializeData()
    {
        var datasets = this._datasets;
        return _dataSerializerService.SerializeData(datasets);
    }

    public string DeserializeData(string DatasetName)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset != null)
        {
            return _dataSerializerService.SerializeData(dataset);
        }
        return string.Empty;
    }
}
