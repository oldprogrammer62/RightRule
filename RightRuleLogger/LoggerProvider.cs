using Microsoft.Extensions.Logging;

namespace RightRuleLogger;

public class LoggerProvider : ILoggerProvider
{
    private readonly string _filePath;

    public LoggerProvider(string filePath)
    {
        this._filePath = filePath;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new Logger(categoryName, _filePath);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
