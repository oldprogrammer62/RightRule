using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                var value = serviceProvider
                    .GetService<IConfiguration>()?["Logging:LogLevel:Default"];

                if (Enum.TryParse<LogLevel>(
                        value,
                        ignoreCase: true,
                        out var level))
                {
                    options.MinLevel = level;
                }
            });
        return builder;
    }
}