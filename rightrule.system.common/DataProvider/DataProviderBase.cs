namespace RightRule.System.Common.DataProvider;

public abstract class DataProviderBase<TInput> : IDataProvider<TInput>
{
    ProviderDataStructure IDataProvider.Process(string DatasetName, object Data)
    {
        if (Data is not TInput typedInput)
        {
            throw new ArgumentException($"Expected type '{typeof(TInput).FullName}', but received '{Data?.GetType().FullName ?? "null"}'.");
        }
        return Process(DatasetName, typedInput);
    }

    public abstract ProviderDataStructure Process(string DatasetName, TInput Data);
}
