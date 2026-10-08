using RightRule.DataProvider.DataProviderLibrary;
using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider;

internal class DataProviderService : IDataProviderService
{
    private readonly IDataProviderLibraryManager _dataProviderLibraryManager;

    public IEnumerable<Type> RegisteredDataProviders => _dataProviderLibraryManager.RegisteredDataProviders;

    public DataProviderService(IDataProviderLibraryManager dataProviderLibraryManager)
    {
        _dataProviderLibraryManager = dataProviderLibraryManager;
    }

    public void RegisterDataProvider(IDataProvider Provider)
    {
        _dataProviderLibraryManager.RegisterDataProvider(Provider);
    }

    public ProviderDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull
    {
        return _dataProviderLibraryManager.ProcessIncomingData(DatasetName, Data);
    }

}
