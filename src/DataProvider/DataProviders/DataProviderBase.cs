using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

public abstract class DataProviderBase<TInput> : IDataProvider<TInput>
{
    InternalDataStructure IDataProvider.Process(string DatasetName, object Data)
    {
        if (Data is not TInput typedInput)
        {
            throw new ArgumentException($"Expected type '{typeof(TInput).FullName}', but received '{Data?.GetType().FullName ?? "null"}'.");
        }
        return Process(DatasetName, typedInput);
    }

    public abstract InternalDataStructure Process(string DatasetName, TInput Data);
}
