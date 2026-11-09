using Microsoft.Extensions.Logging;
using RightData.DataServices.DataLoader.Json;

namespace RightRule.Factories.DataLoaderFactory;

internal sealed class DataLoaderFactory : IDataLoaderFactory
{
    private readonly ILogger<DataLoaderFactory> _logger;

    public DataLoaderFactory(ILogger<DataLoaderFactory> logger)
    {
        _logger = logger;
    }

    public IDataLoader? BuildDataLoader(string DataLoaderType)
    {
        switch (DataLoaderType.ToUpper())
        {
            case "JSON":
                return new JsonDataLoader();
        }
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError("{LoaderType} - Not available", DataLoaderType);
        }
        return null;
    }
}
