using RightRule.DataProvider.DataProviders;
using RightRule.DataProviders;
using System.Collections.Concurrent;

namespace RightRule.DataProvider.DataProviderLibrary;

public class DataProviderLibraryManager : IDataProviderLibraryManager
{
    private readonly ConcurrentDictionary<Type, IDataProvider> _activeDataMappings = new ConcurrentDictionary<Type, IDataProvider>();

    public DataProviderLibraryManager()
    {
        RegisterDataProvider(new DictionaryDataProvider());
        RegisterDataProvider(new StringDataProvider());
        RegisterDataProvider(new IntegerDataProvider());
    }

    public void RegisterDataProvider(object externalProvider)
    {
        if (externalProvider == null) throw new ArgumentNullException(nameof(externalProvider));

        // Look for the generic IDataProvider<TInput> interface implemented by the external class
        var genericInterface = externalProvider.GetType()
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDataProvider<>));

        if (genericInterface == null)
        {
            throw new ArgumentException(
                $"The provided class '{externalProvider.GetType().Name}' does not implement IDataProvider<TInput>.",
                nameof(externalProvider)
            );
        }

        // Extract the 'TInput' type argument from the generic interface definition at runtime
        Type discoveredDataType = genericInterface.GetGenericArguments()[0];

        if (externalProvider is IDataProvider providerInstance)
        {
            // Map the discovered data type to the untyped provider instance
            _activeDataMappings[discoveredDataType] = providerInstance;
        }
        else
        {
            throw new InvalidOperationException($"Provider must implement the core IDataProvider interface.");
        }
    }

    public InternalDataStructure ProcessIncomingData<TInput>(string DS_Name, TInput data) where TInput : notnull
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        // Instantly look up the provider based on the compile-time/inferred type argument
        if (!_activeDataMappings.TryGetValue(typeof(TInput), out var provider))
        {
            throw new KeyNotFoundException($"The data type '{typeof(TInput).FullName}' has not been registered via RegisterData<T>().");
        }

        // Route directly to the provider
        return provider.Process(DS_Name, data);
    }
}
