using Microsoft.Extensions.Logging;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.Models;
using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataServices;

internal sealed partial class DataService : IDataService
{
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
        var dataRegistrationResult = RegisterData(DatasetName, dataResult.Data);

        if (dataRegistrationResult.Status == RightRuleStatusEnum.SUCCESS)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message: "Data Registration Success for Datset: {datasetName}", DatasetName);
            }
        }
        return dataRegistrationResult;
    }

    private DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems)
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
    internal Dataset AddData(string DatasetName, Dictionary<string, object> DataItems)
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
    internal Dataset AddData(string DatasetName, ValueTuple<string, object> DataItem)
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
    internal Dataset AddData(string DatasetName, List<ValueTuple<string, object>> DataItems)
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
    internal Dataset AddData(string DatasetName, string DataItemName, object DataItemValue)
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

        if (Datasets == null)
        {
            return results;
        }

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

    private Dataset CreateDataSet(string DatasetName)
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
