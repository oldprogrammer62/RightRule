using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace RightRuleLogger;

public static class LoggerExtensions
{
    public static ILoggingBuilder AddRightRuleLogger(this ILoggingBuilder builder, string filePath)
    {
        // Register the provider into the DI container
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>(
                sp => new LoggerProvider(filePath)
            )
        );
        return builder;
    }
}
