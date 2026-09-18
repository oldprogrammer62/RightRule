using RightRule.RegistrationServices.DataRegistration.Data;
using RightRule.RegistrationServices.DataRegistration.DataValidator;

namespace RightRule.RegistrationServices.DataRegistration;

internal class DataService : IDataService
{
    private List<Dataset> _datasets;
    private readonly IDataValidator _dataValidator;

    public DataService(IDataValidator dataValidator)
    {
        _datasets = new List<Dataset>();
        _dataValidator = dataValidator;
    }

    public void Clear()
    {
        if (_datasets == null || _datasets.Count == 0)
        {
            return;
        }
        _datasets = new List<Dataset>();
    }

    public bool IsValidData(string name)
    {
        return _dataValidator.IsValidData(this, name);
    }

    public List<string> GetDataItemNames(Dataset dataset)
    {
        return dataset.DataItemNames;
    }

    public List<string> GetDataItemNames()
    {
        List<string> names = new List<string>();
        foreach (Dataset dataset in _datasets)
        {
            names.AddRange(dataset.DataItemNames);
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


    public void DeregisterData(string DatasetName)
    {
        if (_datasets == null || _datasets.Count == 0)
        {
            return;
        }
        _datasets.Remove(_datasets.First(x => x.DatasetName == DatasetName));
    }

    public Dataset CreateDataSet(string DatasetName)
    {
        var dataset = new Dataset(DatasetName);
        _datasets.Add(dataset);
        return dataset;
    }

    // Add Dictionary of data
    // Will be discover as DatasetName.Key
    public Dataset AddData(string DatasetName, Dictionary<string, object> DataItems)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItems(DataItems);
        return dataset;
    }

    // Add Tuple of data
    // Will be discover as DatasetName.Key
    public Dataset AddData(string DatasetName, ValueTuple<string, object> DataItem)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItem(DataItem.Item1, DataItem.Item2);
        return dataset;
    }

    // Add List of Tuples of data
    // Will be discover as DatasetName.Key
    public Dataset AddData(string DatasetName, List<ValueTuple<string, object>> DataItems)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItems(DataItems);
        return dataset;
    }

    // Add Variable by name
    // Will be discover as DatasetName.Key
    public Dataset AddData(string DatasetName, string DataItemName, object DataItemValue)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItem(DataItemName, DataItemValue);
        return dataset;
    }

    // Add Variable
    // Will be discover as Key
    public Dataset AddData(string DatasetName, object DataItemValue)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            dataset = CreateDataSet(DatasetName);
        }
        dataset.AddDataItem(DatasetName, DataItemValue);
        return dataset;
    }

    public Dataset? GetDataset(string DataName)
    {
        return _datasets.FirstOrDefault(d => d.DatasetName.Equals(DataName, StringComparison.Ordinal));
    }

    public List<Dataset>? GetDatasets()
    {
        return _datasets;
    }

    public object? GetDataValue(string DataName)
    {
        var di = GetDataItem(DataName);
        if (di != null)
        {
            return di.Value;
        }
        return null;
    }

    public DataItem? GetDataItem(string DataName)
    {
        foreach (var ds in _datasets)
        {
            var value = ds.GetDataItem(DataName);
            if (value != null)
            {
                return value;
            }
        }
        return null;
    }

    public DataItem? GetDataItem(string DatasetName, string DataName)
    {
        var ds = GetDataset(DatasetName);
        if (ds != null)
        {
            return ds.GetDataItem(DataName);
        }
        return null;
    }

    public object? GetDataValue(string DatasetName, string DataName)
    {
        var ds = GetDataset(DatasetName);
        if (ds != null)
        {
            var di = ds.GetDataItem(DataName);
            if (di != null)
            {
                return di.Value;
            }
        }
        return null;
    }
}
