using RightRule.DataProviders;

namespace RightRule.DataProvider.DataProviderLibrary
{
    public interface IDataProviderLibraryManager
    {
        InternalDataStructure ProcessIncomingData<TInput>(string DS_Name, TInput data) where TInput : notnull;
        void RegisterDataProvider(object externalProvider);
    }
}