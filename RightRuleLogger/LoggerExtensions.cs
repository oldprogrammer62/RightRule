using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace RightRuleLogger;

public static class LoggerExtensions
{
    public static ILoggingBuilder AddRightRuleLogger(
        this ILoggingBuilder builder)
    {
        try
        {
            builder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new LoggerProvider(serviceProvider.GetService<IConfiguration>()));

            return builder;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Configuration Not Found. Failed to add Logger.", ex);
        }
    }
}
