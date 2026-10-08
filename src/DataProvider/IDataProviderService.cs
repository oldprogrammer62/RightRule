using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider
{
    internal interface IDataProviderService
    {
        IEnumerable<Type> RegisteredDataProviders { get; }
        
        ProviderDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull;

        DataProviderRegistrationResult RegisterDataProvider(IDataProvider Provider);
    }
}