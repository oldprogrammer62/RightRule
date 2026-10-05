using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviders;

public abstract class DataProviderBase<TInput> : IDataProvider<TInput>
{
    InternalDataStructure IDataProvider.Process(string DS_Name, object input)
    {
        if (input is not TInput typedInput)
        {
            throw new ArgumentException($"Expected type '{typeof(TInput).FullName}', but received '{input?.GetType().FullName ?? "null"}'.");
        }
        return Process(DS_Name, typedInput);
    }

    public abstract InternalDataStructure Process(string DS_Name, TInput input);
}
