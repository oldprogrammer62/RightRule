using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviderLibrary
{
    public interface IDataProviderLibraryManager
    {
        ProviderDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull;

        void RegisterDataProvider(object ExternalProvider);
    }
}