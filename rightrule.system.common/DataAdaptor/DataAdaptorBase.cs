namespace RightRule.System.Common.DataAdaptor;

public abstract class DataAdaptorBase<TInput> : IDataProvider<TInput>
{
    AdaptorDataStructure IDataAdaptor.Process(string DatasetName, object Data)
    {
        if (Data is not TInput typedInput)
        {
            throw new ArgumentException($"Expected type '{typeof(TInput).FullName}', but received '{Data?.GetType().FullName ?? "null"}'.");
        }
        return Process(DatasetName, typedInput);
    }

    public abstract AdaptorDataStructure Process(string DatasetName, TInput Data);
}
