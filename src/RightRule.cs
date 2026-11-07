using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RightRuleService;
using RightRule.RuleServices;
using System.ComponentModel;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private bool Disposed { get; set; } = false;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly IRightRuleService _rightRuleService;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly ILogger<RightRule> _logger;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly IServiceProvider? _serviceProvider;


    public IRuleService? RuleService => _rightRuleService.RuleService;

    public IDataService? DataService => _rightRuleService.DataService;

    public IOperatorService? OperatorService => _rightRuleService.OperatorService;

    public IDataAdaptorService? DataAdaptorService => _rightRuleService.DataAdaptorService;

    private readonly RightRuleOptions _options;
    public RightRuleOptions Options => GetRightRuleOptions();

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
        _serviceProvider = InitialiseServiceProvider(
            _options,
            loggerFactory
            );

        _rightRuleService = GetRightRuleServiceFromDI();

        _logger = GetLoggerServiceFromDI();
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("new RightRule() Started");
        }

        // Log Options
        LogOptions(_logger, options);
    }

    /// <summary>
    /// Called by DI container hosted by calling application
    /// </summary>
    /// <param name = "rightRuleService" ></param >
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public RightRule(
        IRightRuleService rightRuleService,
        ILogger<RightRule>? logger,
        RightRuleOptions options
        )
    {
        _options = options;
        _rightRuleService = rightRuleService;

        _logger = logger ?? NullLogger<RightRule>.Instance;
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("RightRule DI Service Started");
        }

        // Log Options
        LogOptions(_logger, options);
    }

    public RightRuleOptions GetRightRuleOptions()
    {
        return _options;
    }

    private static ServiceProvider InitialiseServiceProvider(
        RightRuleOptions options,
        ILoggerFactory loggerFactory
        )
    {
        options = CheckOptions(options);

        var services = new ServiceCollection();

        services.AddSingleton(options);
        services.AddSingleton<ILoggerFactory>(loggerFactory);

        ServiceRegister.ServiceRegister.RegisterServices(services);

        return services.BuildServiceProvider();
    }

    private IRightRuleService GetRightRuleServiceFromDI()
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider is not initialized.");
        }
        return _serviceProvider.GetRequiredService<IRightRuleService>();
    }

    private ILogger<RightRule> GetLoggerServiceFromDI()
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider is not initialized.");
        }
        return _serviceProvider.GetRequiredService<ILogger<RightRule>>();
    }

    private static void LogOptions(ILogger<RightRule> logger, RightRuleOptions options)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "Creating RightRule with options: DataCheck={DataCheck}, " +
                "Diagnostics={Diagnostics}, " +
                "LoadCustomOperators={LoadCustomOperators}, " +
                "LoadDefaultOperators={LoadDefaultOperators}, " +
                "CustomOperatorLibraryPath={CustomOperatorLibraryPath}",
                options.DataCheck,
                options.Diagnostics,
                options.LoadCustomOperators,
                options.LoadDefaultOperators,
                options.CustomOperatorLibraryPath);
        }
    }

    public void Dispose()
    {
        if (Disposed) return;

        if (_serviceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
        if (_rightRuleService is IDisposable disposableRightRuleService)
        {
            disposableRightRuleService.Dispose();
        }
        Disposed = true;
    }
}
