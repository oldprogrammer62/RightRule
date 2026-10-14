using System.Collections.Concurrent;
using RightRule.DataAdaptors.DataAdaptors;
using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptorLibrary;

public class DataAdaptorLibraryManager : IDataAdaptorLibraryManager
{
    private readonly ConcurrentDictionary<Type, IDataAdaptor> _activeDataMappings = new ConcurrentDictionary<Type, IDataAdaptor>();

    public IEnumerable<Type> RegisteredDataAdaptors => _activeDataMappings.Keys.ToList();

    /// <summary>
    /// Register the default data providers for Dictionary, String, and Object types.
    /// </summary>
    public DataAdaptorLibraryManager()
    {
        RegisterDataAdaptor(new DictionaryDataAdaptor());
        RegisterDataAdaptor(new StringDataAdaptor());
        RegisterDataAdaptor(new TupleStringIntDataAdaptor());
        RegisterDataAdaptor(new ListTupleStringIntDataAdaptor());
        RegisterDataAdaptor(new ObjectDataAdaptor());
    }

    public DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor DataAdaptor)
    {
        if (DataAdaptor == null)
        {
            return DataAdaptorRegistrationResult.Failed($"Adaptor is null");
        }

        // Look for the generic IDataAdaptor<TInput> interface implemented by the Adaptor class
        var genericInterface = DataAdaptor.GetType()
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDataProvider<>));

        if (genericInterface == null)
        {
            return DataAdaptorRegistrationResult.Failed($"The provided class '{DataAdaptor.GetType().Name}' does not implement IDataAdaptor<TInput>.");
        }

        // Extract the 'TInput' type argument from the generic interface definition at runtime
        Type discoveredDataType = genericInterface.GetGenericArguments()[0];

        if (DataAdaptor is IDataAdaptor adaptorInstance)
        {
            // Map the discovered data type to the untyped adaptor instance
            _activeDataMappings[discoveredDataType] = adaptorInstance;
        }
        else
        {
            return DataAdaptorRegistrationResult.Failed($"The provided class '{DataAdaptor.GetType().Name}' does not implement the core IDataAdaptor interface.");
        }
        return DataAdaptorRegistrationResult.Success($"DataAdaptor registered successfully.");
    }

    public AdaptorDataStructure ProcessIncomingData<TInput>(string DatasetName, TInput Data) where TInput : notnull
    {
        if (Data == null) throw new ArgumentNullException(nameof(Data));

        // Instantly look up the adaptor based on the compile-time/inferred type argument
        if (!_activeDataMappings.TryGetValue(typeof(TInput), out IDataAdaptor? adaptor))
        {
            adaptor = _activeDataMappings.GetValueOrDefault(typeof(object));
            if (adaptor == null)
            {
                throw new KeyNotFoundException($"The data type '{typeof(TInput).FullName}' has not been registered via RegisterData<T>().");
            }
        }

        // Route directly to the adaptor
        return adaptor.Process(DatasetName, Data);
    }
}
