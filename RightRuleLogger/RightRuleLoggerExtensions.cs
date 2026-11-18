using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace RightRuleLogger;

public static class RightRuleLoggerExtensions
{
    public static ILoggingBuilder AddRightRuleLogger(this ILoggingBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerProvider>(
            serviceProvider => new RightRuleLoggerProvider(
                serviceProvider.GetService<IConfiguration>()));

        // Read configuration when the logging options are resolved,
        // after the service provider has been built.
        builder.Services.AddOptions<LoggerFilterOptions>()
            .Configure<IServiceProvider>((options, serviceProvider) =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();

                var globalLevelText = configuration?["Logging:LogLevel:Default"];
                if (Enum.TryParse<LogLevel>(
                        globalLevelText,
                        ignoreCase: true,
                        out var globalLevel))
                {
                    options.MinLevel = globalLevel;
                }

                var providerLevelText =
                    configuration?["Logging:RightRuleLogger:LogLevel:Default"];

                LogLevel? providerLevel = null;
                if (Enum.TryParse<LogLevel>(
                        providerLevelText,
                        ignoreCase: true,
                        out var parsedProviderLevel))
                {
                    providerLevel = parsedProviderLevel;
                    options.Rules.Add(new LoggerFilterRule(
                        "RightRuleLogger",
                        categoryName: null,
                        logLevel: parsedProviderLevel,
                        filter: null));
                }

                Trace.WriteLine(
                    $"RightRuleLogger settings applied: " +
                    $"GlobalLogLevel={globalLevelText ?? "<not set>"}; " +
                    $"ProviderLogLevel={providerLevelText ?? "<not set>"}; " +
                    $"EffectiveProviderLogLevel={providerLevel?.ToString() ?? options.MinLevel.ToString()}; " +
                    $"LogFile:Path={configuration?["LogFile:Path"] ?? "<default>"}; " +
                    $"LogFile:Name={configuration?["LogFile:Name"] ?? "<default>"}");
            });

        return builder;
    }
}
