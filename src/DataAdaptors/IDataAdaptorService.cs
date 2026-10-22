using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors;

public interface IDataAdaptorService
{
    IEnumerable<Type> RegisteredDataAdaptors { get; }
    
    AdaptorDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull;

    DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor Adaptor);
}