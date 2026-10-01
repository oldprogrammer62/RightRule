using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RightRuleLogger;

public class LoggerProvider : ILoggerProvider
{
    private readonly IConfiguration? _configuration;

    public LoggerProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new Logger(_configuration, categoryName);
    }

    public void Dispose()
    {
    }
}
