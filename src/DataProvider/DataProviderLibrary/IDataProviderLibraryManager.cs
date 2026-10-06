using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviderLibrary
{
    public interface IDataProviderLibraryManager
    {
        InternalDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull;

        void RegisterDataProvider(object ExternalProvider);
    }
}