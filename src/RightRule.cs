using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RightRule.Models;
using RightRule.RightRuleSession;

namespace RightRule;

public sealed partial class RightRule : IRightRule
{
    private readonly RightRuleOptions _options;
    private readonly ILoggerFactory _loggerFactory;

    public RightRule() : 
        this(new RightRuleOptions(), NullLoggerFactory.Instance)
    {
    }

    public RightRule(RightRuleOptions options) :
        this(options, NullLoggerFactory.Instance)
    {
    }

    public RightRule(ILoggerFactory loggerFactory) : 
        this(new RightRuleOptions(), loggerFactory)
    {
    }

    public RightRule(
        RightRuleOptions options, 
        ILoggerFactory loggerFactory
        )
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _options = options;
        _loggerFactory = loggerFactory;
    }

    public IRightRuleSession CreateSession()
    {
        var sessionOptions = new RightRuleOptions
        {
            DataCheck = _options.DataCheck,
            Diagnostics = _options.Diagnostics,
            LoadCustomOperators = _options.LoadCustomOperators,
            LoadDefaultOperators = _options.LoadDefaultOperators,
            CustomOperatorLibraryPath = _options.CustomOperatorLibraryPath
        };

        if (!CheckOptions(sessionOptions))
        {
            throw new ArgumentException("Invalid RightRuleOptions", nameof(sessionOptions));
        }

        var provider = InitialiseServiceProvider(sessionOptions, _loggerFactory);
        try
        {
            return new RightRuleSession.RightRuleSession(provider, ownsServiceProvider: true);
        }
        catch
        {
            provider.Dispose();
            throw;
        }
    }

    private static ServiceProvider InitialiseServiceProvider(RightRuleOptions options, ILoggerFactory loggerFactory)
    {
        var services = new ServiceCollection();

        services.AddSingleton(options);
        services.AddSingleton<ILoggerFactory>(loggerFactory);

        ServiceRegister.ServiceRegister.RegisterServices(services);
        return services.BuildServiceProvider();
    }
}
