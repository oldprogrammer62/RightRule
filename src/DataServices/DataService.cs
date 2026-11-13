using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataLoader;
using RightRule.DataServices.DataRegistration;
using RightRule.DataServices.DataSerializer;
using RightRule.DataServices.DataValidator;
using RightRule.Models;
using RightRule.System.Common.DataAdaptor;
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

    public List<DataRegistrationResult> LoadData(string JsonData)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(JsonData, nameof(JsonData));

        var dataDef = _dataLoaderService.LoadData(JsonData);
        if (dataDef == null)
        {
            return [];
        }
        return AddData(dataDef);
    }

    public bool DeRegisterData()
    {
        if (_datasets == null || _datasets.Count == 0)
        {
            return true;
        }
        _datasets = new List<Dataset>();

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Datasets Cleared");
        }
        return true;
    }

    public bool DeRegisterData(string DatasetName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        if (_datasets == null || _datasets.Count == 0)
        {
            return true;
        }
        _datasets.Remove(_datasets.First(x => x.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase)));

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("{DatasetName} deregistered", DatasetName);
        }
        return true;
    }

    public bool IsValidDataIdentifier(string DataIdentifier)
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

    public DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull
    {
        if (_dataAdaptorService == null)
        {
            return new DataRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }

        AdaptorDataStructure dataResult = _dataAdaptorService.ProcessIncomingData<T>(DatasetName, Data);
        if (dataResult == null)
        {
            return new DataRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }
        var dataRegistrationResult = RegisterIncomingData(DatasetName, dataResult.Data);

        if (dataRegistrationResult.Status == RightRuleStatusEnum.SUCCESS)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message: "Data Registration Success for Datset: {datasetName}", DatasetName);
            }
        }
        return dataRegistrationResult;
    }

    private DataRegistrationResult RegisterIncomingData(string DatasetName, Dictionary<string, object> DataItems)
    {
        ArgumentNullException.ThrowIfNull(DataItems, nameof(DataItems));
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));

        // Add data to new Data Service
        var added = AddData(DatasetName, DataItems);
        if (added == null)
        {
            return new DataRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                DataType = DataTypes.DATASET,
                Name = DatasetName
            };
        }
        return new DataRegistrationResult()
        {
            Status = RightRuleStatusEnum.SUCCESS,
            DataType = DataTypes.DATASET,
            Name = DatasetName
        };
    }

    // Add Dictionary of data
    // Will be discover as DatasetName.Identifier
    public Dataset AddData(string DatasetName, Dictionary<string, object> DataItems)
    {
        var dataset = GetRegisteredData(DatasetName);
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
        var dataset = GetRegisteredData(DatasetName);
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
        var dataset = GetRegisteredData(DatasetName);
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
        ArgumentException.ThrowIfNullOrWhiteSpace(DatasetName, nameof(DatasetName));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataItemName, nameof(DataItemName));

        var dataset = GetRegisteredData(DatasetName);
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
