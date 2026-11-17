using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RightRule.Models;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule
{
    private readonly RightRuleOptions _options;
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Parameterless constructor
    /// Called by: new RightRule();
    /// Service Provider DI Container is hosted by RightRule
    /// Service Provider DI Container will be disposed when RightRule is disposed
    /// </summary>
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
            return new RightRuleSession(provider, ownsServiceProvider: true);
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
