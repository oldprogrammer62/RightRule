using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

public interface IDataProvider
{
    InternalDataStructure Process(string DS_Name, object input);
}

public interface IDataProvider<TInput> : IDataProvider
{
    InternalDataStructure Process(string DS_Name, TInput input);
}
