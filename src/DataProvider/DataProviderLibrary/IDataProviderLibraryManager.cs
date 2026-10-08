using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviderLibrary
{
    public interface IDataProviderLibraryManager
    {
        IEnumerable<Type> RegisteredDataProviders { get; }

        ProviderDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull;

        void RegisterDataProvider(IDataProvider DataProvider);
    }
}