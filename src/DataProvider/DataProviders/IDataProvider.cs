using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

public interface IDataProvider
{
    InternalDataStructure Process(string DatasetName, object Data);
}

public interface IDataProvider<TInput> : IDataProvider
{
    InternalDataStructure Process(string DatasetName, TInput Data);
}
