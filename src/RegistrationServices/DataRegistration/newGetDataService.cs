using RightRule.Data;

namespace RightRule.RegistrationServices.DataRegistration;

// TO DO Wants to be called DataService
internal class newGetDataService : InewGetDataService
{
    public DataService _registeredData { get; init; }

    public newGetDataService()
    {
        _registeredData = new DataService();
    }

    public void Clear()
    {
        if (_registeredData.RegisteredData.Count == 0)
        {
            return;
        }
        _registeredData.Clear();
    }

    public void DeregisterData(string DatasetName)
    {
        if (_registeredData.RegisteredData.Count == 0)
        {
            return;
        }
        _registeredData.DeregisterData(DatasetName);
    }


    public bool newAddData(string DatasetName, Dictionary<string, object> DataItems)
    {
        return _registeredData.AddData(DatasetName, DataItems);
    }

    public bool newAddData(string DatasetName, ValueTuple<string, object> DataItem)
    {
        return _registeredData.AddData(DatasetName, DataItem);
    }

    public bool newAddData(string DatasetName, string DataItemName, object DataItemValue)
    {
        return _registeredData.AddData(DatasetName, DataItemName, DataItemValue);
    }

    public List<newRegisteredData>? newGetData()
    {
        return _registeredData.GetRegisteredData();
    }

    public newRegisteredData? newGetData(string DataName)
    {
        if (string.IsNullOrWhiteSpace(DataName))
        {
            return default;
        }
        return _registeredData.GetRegisteredData(DataName);
    }

    public object? newGetDataValue(string LeftSide, string RightSide)
    {
        var dataSet = GetRegisteredData(LeftSide);
        if (dataSet == null) 
        {
            return default;
        }
        var dataValue = GetDataValue(dataSet, RightSide);
        return dataValue;
    }

    private newRegisteredData? GetRegisteredData(string name)
    {
        return _registeredData.GetRegisteredData(name);
    }

    private newDataItem? GetDataItem(newRegisteredData Source, string Dataname)
    {
        return Source.GetDataItem(Dataname);
    }

    private object? GetDataValue(newRegisteredData Source, string Dataname)
    {
        return Source.GetDataValue(Dataname);
    }

    //public List<RegisteredData> GetData(IRegisteredDatasets? _registeredDataSets, IRegisteredDataObjects? _registeredDataObjects)
    //{
    //    if (_registeredDataSets != null)
    //    {
    //        foreach (var ds in _registeredDataSets.Datasets)
    //        {
    //            RegisteredData rd = new RegisteredData();
    //            rd.MergeData(ds);
    //            DataItem.Add(rd);
    //        }
    //    }

    //    if (_registeredDataObjects != null)
    //    {
    //        foreach (var ds in _registeredDataObjects.Objects)
    //        {
    //            RegisteredData rd = new RegisteredData();
    //            rd.MergeData(ds);
    //            Datas.Add(rd);
    //        }
    //    }
    //    return Datas;
    //}
}
