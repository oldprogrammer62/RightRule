using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace RightRuleLogger;

public sealed class RightRuleLogger<TCategoryName> : RightRuleLogger, ILogger<TCategoryName>
{
}

public class RightRuleLogger : ILogger
{
    private readonly string _categoryName = "RightRule";
    private readonly string _filePath;
    private const string _DEFAULTFILEPATH = "rightrulelogs\\";
    private const string _DEFAULTFILENAME = "rightrule.log";

    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);

    public RightRuleLogger()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        _filePath = MakeFilePath(configuration);
    }

    public RightRuleLogger(IConfiguration? configuration, string catgoryName)
    {
        _filePath = MakeFilePath(configuration);
    }

    private string MakeFilePath(IConfiguration? configuration)
    {
        var filePath = configuration?["File:Path"] ?? Path.Combine(AppContext.BaseDirectory, _DEFAULTFILEPATH);
        var fileName = configuration?["File:Name"] ?? _DEFAULTFILENAME;
        return Path.Combine(filePath, fileName);
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
        return default;
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
