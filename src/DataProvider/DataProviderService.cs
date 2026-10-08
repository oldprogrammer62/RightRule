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

    public DataProviderRegistrationResult RegisterDataProvider(IDataProvider Provider)
    {
        return _dataProviderLibraryManager.RegisterDataProvider(Provider);
    }

    public ProviderDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull
    {
        return _dataProviderLibraryManager.ProcessIncomingData(DatasetName, Data);
    }
}
