using Microsoft.Extensions.Logging;
using RightRule.DataServices.DataLoaders.Json;

namespace RightRule.Factories.RuleLoaderFactory;

internal sealed class RuleLoaderFactory : IRuleLoaderFactory
{
    private readonly ILogger<RuleLoaderFactory> _logger;

    public RuleLoaderFactory(ILogger<RuleLoaderFactory> logger)
    {
        _logger = logger;
    }

    public IRuleLoader? BuildLoader(string LoaderType)
    {
        switch (LoaderType.ToUpper())
        {
            case "JSON":
                return new JsonLoader();
        }
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError("{LoaderType} - Not available", LoaderType);
        }
        return null;
    }
}
