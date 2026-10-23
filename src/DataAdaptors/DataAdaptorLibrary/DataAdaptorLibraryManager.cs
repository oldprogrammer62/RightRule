using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors.DataAdaptors;
using RightRule.System.Common.DataAdaptor;

namespace RightRule.DataAdaptors.DataAdaptorLibrary;

internal sealed class DataAdaptorLibraryManager : IDataAdaptorLibraryManager
{
    private readonly ConcurrentDictionary<Type, IDataAdaptor> _activeDataMappings = new();
    public IEnumerable<Type> RegisteredDataAdaptors => [.. _activeDataMappings.Keys];

    private readonly ILogger<DataAdaptorLibraryManager> _logger;

    /// <summary>
    /// Register the default data providers for Dictionary, String, and Object types.
    /// </summary>
    public DataAdaptorLibraryManager(ILogger<DataAdaptorLibraryManager> logger)
    {
        _logger = logger;

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

            if (_logger.IsEnabled(LogLevel.Information))
            {
                var adaptorName = adaptorInstance.AdaptorName;
                _logger.LogInformation("DataAdaptor Registered: {adaptorname}", adaptorName);
            }
        }
        else
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                var adaptorName = DataAdaptor.GetType().Name;
                _logger.LogInformation("DataAdaptor Not Registered: {adaptorname}. Class does not implement IDataAdaptor", adaptorName);
            }

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
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    var dataType = typeof(TInput).FullName;
                    _logger.Log(LogLevel.Warning, "The data type '{dataType}' has not been registered via RegisterData<T>().", dataType);
                }
                throw new KeyNotFoundException($"The data type '{typeof(TInput).FullName}' has not been registered via RegisterData<T>().");
            }
        }

        // Route directly to the adaptor
        return ExecuteAdaptorProcess(adaptor, DatasetName, Data);
    }

    private AdaptorDataStructure ExecuteAdaptorProcess<TInput>(IDataAdaptor adaptor, string DatasetName, TInput Data)
    {
        if (Data is null)
        {
            return new AdaptorDataStructure();
        }
        try
        {
            return adaptor.Process(DatasetName, Data);
        }
        catch
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError("DataAdaptor - Error processing data. DatasetName: {}", DatasetName);
            }
            return new AdaptorDataStructure();
        }
    }
}
