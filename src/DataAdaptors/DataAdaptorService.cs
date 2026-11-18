using RightRule.DataAdaptors.DataAdaptorLibrary;
using RightRule.System.Common.DataAdaptor;
using System.Diagnostics;

namespace RightRule.DataAdaptors;

internal sealed class DataAdaptorService : IDataAdaptorService
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IDataAdaptorLibraryManager _dataProviderLibraryManager;

    public IEnumerable<Type> RegisteredDataAdaptors => _dataProviderLibraryManager.RegisteredDataAdaptors;

    public DataAdaptorService(IDataAdaptorLibraryManager dataProviderLibraryManager)
    {
        _dataProviderLibraryManager = dataProviderLibraryManager;
    }

    public DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor Adaptor)
    {
        return _dataProviderLibraryManager.RegisterDataAdaptor(Adaptor);
    }

    public AdaptorDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull
    {
        return _dataProviderLibraryManager.ProcessIncomingData(DatasetName, Data);
    }
}
