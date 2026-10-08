namespace RightRule.System.Common.DataProvider;

public interface IDataProvider
{
    ProviderDataStructure Process(string DatasetName, object Data);
}

public interface IDataProvider<TInput> : IDataProvider
{
    ProviderDataStructure Process(string DatasetName, TInput Data);
}
