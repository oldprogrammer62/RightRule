using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RightRuleLogger;

public class RightRuleLoggerProvider : ILoggerProvider
{
    private readonly IConfiguration? _configuration;

    public RightRuleLoggerProvider(IConfiguration? configuration)
    {
        _configuration = configuration;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new RightRuleLogger(_configuration, categoryName);
    }

    public void Dispose()
    {
    }
}
