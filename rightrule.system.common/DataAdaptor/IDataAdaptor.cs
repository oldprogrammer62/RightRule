namespace RightRule.System.Common.DataAdaptor;

public interface IDataAdaptor
{
    string AdaptorName { get; init; }

    AdaptorDataStructure Process(string DatasetName, object Data);
}

public interface IDataProvider<TInput> : IDataAdaptor
{
    AdaptorDataStructure Process(string DatasetName, TInput Data);
}
