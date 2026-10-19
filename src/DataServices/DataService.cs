using Microsoft.Extensions.Logging;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataValidator;

namespace RightRule.DataServices;

internal class DataService : IDataService
{
    private List<Dataset> _Datasets;
    private readonly ILogger<DataService> _logger;
    private readonly IDataValidatorService _DataValidator;

    public DataService(ILogger<DataService> logger, IDataValidatorService DataValidator)
    {
        _logger = logger;

        _Datasets = new List<Dataset>();
        _DataValidator = DataValidator;
    }

    public void Clear()
    {
        if (_Datasets == null || _Datasets.Count == 0)
        {
            return;
        }
        _Datasets = new List<Dataset>();

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Datasets Cleared");
        }
    }

    public bool IsValidDataIdentifier(string DataIdentifier)
    {
        return _DataValidator.IsValidDataIdentifier(this, DataIdentifier);
    }

    public List<string>? GetDataItemNames(string DatasetName)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            return new List<string>();
        }
        return GetDataItemNames(dataset);
    }

    public List<string>? GetDataItemNames(Dataset Dataset)
    {
        var names = Dataset.DataItemNames;
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var countOfNames = names?.Count ?? 0;
            var datasetName = Dataset.DatasetName;
            _logger.LogInformation("{countOfNames} DataItem Names found in {dataset}", countOfNames, datasetName);
        }
        return names;
    }

    public List<string> GetDataSetNames()
    {
        List<string> names = new List<string>();
        foreach (Dataset dataset in _Datasets)
        {
            names.AddRange(dataset.DatasetName);
        }
        return names;
    }

    public void DeregisterDataset(string DatasetName)
    {
        if (_Datasets == null || _Datasets.Count == 0)
        {
            return;
        }
        _Datasets.Remove(_Datasets.First(x => x.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase)));

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogInformation("{DatasetName} deregistered", DatasetName);
        }
    }

    public Dataset CreateDataSet(string DatasetName)
    {
        var dataset = new Dataset(DatasetName);
        _Datasets.Add(dataset);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogInformation("{DatasetName} created", DatasetName);
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
            var countOfData = DataItems.Count;
            _logger.LogInformation("Dictionary Data. {countOfData} Data Items added to {DatasetName}",
                    countOfData,
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
            var countOfData = 1;
            _logger.LogInformation("(string,object) Data. {countOfData} Data Item added to {DatasetName}",
                    countOfData,
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
            var countOfData = DataItems.Count;
            _logger.LogInformation("List<(string,object)> Data. {countOfData} Data Items added to {DatasetName}",
                    countOfData,
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
            var countOfData = 1;
            _logger.LogInformation("Simple Variable. {countOfData} Data Item added to {DatasetName}",
                    countOfData,
                    DatasetName);
        }

        return dataset;
    }

    public List<Dataset>? GetDatasets()
    {
        return _Datasets;
    }

    public Dataset? GetDataset(string DatasetName)
    {
        return _Datasets.FirstOrDefault(d => d.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase));
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
}
