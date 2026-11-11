using Microsoft.Extensions.Logging;
using RightRule.RuleServices.RuleLoader;
using RightRule.RuleServices.RuleLoader.Json;

namespace RightRule.Factories.RuleLoaderFactory;

internal sealed class RuleLoaderFactory : IRuleLoaderFactory
{
    private readonly ILogger<RuleLoaderFactory> _logger;

    public RuleLoaderFactory(ILogger<RuleLoaderFactory> logger)
    {
        _logger = logger;
    }

    public IRuleLoader? BuildRuleLoader(string RuleLoaderType)
    {
        switch (RuleLoaderType.ToUpper())
        {
            case "JSON":
                return new JsonRuleLoader();
        }
        if (_logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError("Rule Loader: {LoaderType} - Not available", RuleLoaderType);
        }
        return null;
    }
}
