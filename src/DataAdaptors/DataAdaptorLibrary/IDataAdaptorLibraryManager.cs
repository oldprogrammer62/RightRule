using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptorLibrary
{
    public interface IDataAdaptorLibraryManager
    {
        IEnumerable<Type> RegisteredDataAdaptors { get; }

        AdaptorDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull;

        DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor DataAdaptor);
    }
}