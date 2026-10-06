using RightRule.DataServices.Data;
using RightRule.DataServices.DataValidator;

namespace RightRule.DataServices;

internal class DataService : IDataService
{
    private List<Dataset> Datasets;
    private readonly IDataValidatorService DataValidator;

    public DataService(IDataValidatorService DataValidator)
    {
        Datasets = new List<Dataset>();
        this.DataValidator = DataValidator;
    }

    public void Clear()
    {
        if (Datasets == null || Datasets.Count == 0)
        {
            return;
        }
        Datasets = new List<Dataset>();
    }

    public bool IsValidDataIdentifier(string DataIdentifier)
    {
        return DataValidator.IsValidDataIdentifier(this, DataIdentifier);
    }

    public List<string> GetDataItemNames(string DatasetName)
    {
        var dataset = GetDataset(DatasetName);
        if (dataset == null)
        {
            return new List<string>();
        }
        return GetDataItemNames(dataset);
    }

    public List<string> GetDataItemNames(Dataset Dataset)
    {
        return Dataset.DataItemNames;
    }

    public List<string> GetDataSetNames()
    {
        List<string> names = new List<string>();
        foreach (Dataset dataset in Datasets)
        {
            names.AddRange(dataset.DatasetName);
        }
        return names;
    }

    public void DeregisterDataset(string DatasetName)
    {
        if (Datasets == null || Datasets.Count == 0)
        {
            return;
        }
        Datasets.Remove(Datasets.First(x => x.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase)));
    }

    public Dataset CreateDataSet(string DatasetName)
    {
        var dataset = new Dataset(DatasetName);
        Datasets.Add(dataset);
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
        return dataset;
    }

    public List<Dataset>? GetDatasets()
    {
        return Datasets;
    }

    public Dataset? GetDataset(string DatasetName)
    {
        return Datasets.FirstOrDefault(d => d.DatasetName.Equals(DatasetName, StringComparison.OrdinalIgnoreCase));
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
