using RightRule.DataProvider.DataProviders;
using System.Collections.Concurrent;
using RightRule.System.Common.DataProvider;

namespace RightRule.DataProvider.DataProviderLibrary;

public class DataProviderLibraryManager : IDataProviderLibraryManager
{
    private readonly ConcurrentDictionary<Type, IDataProvider> _activeDataMappings = new ConcurrentDictionary<Type, IDataProvider>();

    /// <summary>
    /// Register the default data providers for Dictionary, String, and Object types.
    /// </summary>
    public DataProviderLibraryManager()
    {
        RegisterDataProvider(new DictionaryDataProvider());
        RegisterDataProvider(new StringDataProvider());
        RegisterDataProvider(new TupleStringIntDataProvider());
        RegisterDataProvider(new ListTupleStringIntDataProvider());
        RegisterDataProvider(new ObjectDataProvider());
    }

    public void RegisterDataProvider(object Provider)
    {
        if (Provider == null) throw new ArgumentNullException(nameof(Provider));

        // Look for the generic IDataProvider<TInput> interface implemented by the Provider class
        var genericInterface = Provider.GetType()
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDataProvider<>));

        if (genericInterface == null)
        {
            throw new ArgumentException(
                $"The provided class '{Provider.GetType().Name}' does not implement IDataProvider<TInput>.",
                nameof(Provider)
            );
        }

        // Extract the 'TInput' type argument from the generic interface definition at runtime
        Type discoveredDataType = genericInterface.GetGenericArguments()[0];

        if (Provider is IDataProvider providerInstance)
        {
            // Map the discovered data type to the untyped provider instance
            _activeDataMappings[discoveredDataType] = providerInstance;
        }
        else
        {
            throw new InvalidOperationException($"Provider must implement the core IDataProvider interface.");
        }
    }

    public ProviderDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull
    {
        if (Data == null) throw new ArgumentNullException(nameof(Data));

        // Instantly look up the provider based on the compile-time/inferred type argument
        if (!_activeDataMappings.TryGetValue(typeof(TInput), out IDataProvider? provider))
        {
            provider = _activeDataMappings.GetValueOrDefault(typeof(object));
            if (provider == null)
            {
                throw new KeyNotFoundException($"The data type '{typeof(TInput).FullName}' has not been registered via RegisterData<T>().");
            }
        }

        // Route directly to the provider
        return provider.Process(DatasetName, Data);
    }
}
