using Microsoft.Extensions.Logging;
using System.Text;

namespace RightRuleLogger;

public class Logger : ILogger
{
    private readonly string _categoryName;
    private string _filePath;

    private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);

    public Logger(string categoryName, string filePath)
    {
        this._categoryName = categoryName;
        this._filePath = CheckFilePath(filePath);
    }

    private string CheckFilePath(string filePath)
    {
        var appPath = Directory.GetCurrentDirectory();
        var directory = Path.GetDirectoryName (filePath);
        var filename = Path.GetFileName(filePath);
        var fullPath = Path.Combine(appPath, directory);
        if (Directory.Exists(fullPath))
        {
            return Path.Combine(fullPath, filename);
        }
        Directory.CreateDirectory(fullPath);
        return Path.Combine(fullPath, filename);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Debug;
    }

    public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        // Get the fully formatted message text
        string message = formatter(state, exception);

        // Structure your log line
        string logLine = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [{logLevel}] [{_categoryName}] {message}";
        if (exception != null)
        {
            logLine += Environment.NewLine + exception.ToString();
        }
        await WriteToLogFile(logLine);
    }

    private async Task WriteToLogFile(string message)
    {
        try
        {
            await _semaphoreSlim.WaitAsync();

            byte[] encodedMessage = Encoding.UTF8.GetBytes(message + Environment.NewLine);
            using FileStream fileStream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.None);
            await fileStream.WriteAsync(encodedMessage);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }
}
