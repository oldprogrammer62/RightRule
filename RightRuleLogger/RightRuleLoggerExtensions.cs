using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace RightRuleLogger;

public static class RightRuleLoggerExtensions
{
    public static ILoggingBuilder AddRightRuleLogger(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerProvider>
            (
            serviceProvider => new RightRuleLoggerProvider(serviceProvider.GetService<IConfiguration>())
            );

        // Read configuration when the logging options are resolved, after
        // the service provider has been built.
        builder.Services.AddOptions<LoggerFilterOptions>()
            .Configure<IServiceProvider>((options, serviceProvider) =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var configuredLevel = configuration?["Logging:LogLevel:Default"];

                if (Enum.TryParse<LogLevel>(
                        configuredLevel,
                        ignoreCase: true,
                        out var level))
                {
                    options.MinLevel = level;
                }

                Trace.WriteLine(
                    $"RightRuleLogger settings applied: " +
                    $"MinLevel={options.MinLevel}; " +
                    $"LogFile:Path={configuration?["LogFile:Path"] ?? "<default>"}; " +
                    $"LogFile:Name={configuration?["LogFile:Name"] ?? "<default>"}");
            });
        return builder;
    }
}