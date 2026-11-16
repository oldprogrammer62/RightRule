using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RightRuleService;
using RightRule.RuleServices;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    private bool Disposed { get; set; } = false;
    private readonly IRightRuleService _rightRuleService;
    private readonly ILogger<RightRule> _logger;
    private readonly IServiceProvider? _serviceProvider;

    private readonly IRuleService _ruleService;
    public IRuleService RuleService => _ruleService;

    private readonly IDataService _dataService;
    public IDataService DataService => _dataService;

    private readonly IDataAdaptorService _dataAdaptorService;
    public IDataAdaptorService DataAdaptorService => _dataAdaptorService;

    private readonly IOperatorService? _operatorService;
    public IOperatorService? OperatorService => _operatorService;

    private readonly RightRuleOptions _rightRuleOptions;
    public RightRuleOptions RightRuleOptions => _rightRuleOptions;

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

        _rightRuleOptions = options;
        _serviceProvider = InitialiseServiceProvider(
            _rightRuleOptions,
            loggerFactory
            );

        _rightRuleService = GetServiceFromDI<IRightRuleService>();
        _ruleService = GetServiceFromDI<IRuleService>();
        _dataService = GetServiceFromDI<IDataService>();
        _dataAdaptorService = GetServiceFromDI<IDataAdaptorService>();
        _operatorService = GetServiceFromDI<IOperatorService>();

        _logger = GetServiceFromDI<ILogger<RightRule>>();
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
    /// <param name="ruleService"></param>
    /// <param name="dataService"></param>
    /// <param name="dataAdaptorService"></param>
    /// <param name="operatorService"></param>
    /// <param name = "rightRuleService" ></param >
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public RightRule(
        IRuleService ruleService,
        IDataService dataService,
        IDataAdaptorService dataAdaptorService,
        IOperatorService operatorService,
        IRightRuleService rightRuleService,
        ILogger<RightRule>? logger,
        RightRuleOptions options
        )
    {
        _rightRuleOptions = options;
        _ruleService = ruleService;
        _dataService = dataService;
        _operatorService = operatorService;
        _dataAdaptorService = dataAdaptorService;
        _rightRuleService = rightRuleService;

        _logger = logger ?? NullLogger<RightRule>.Instance;
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("RightRule DI Service Started");
        }

        // Log Options
        LogOptions(_logger, options);
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

    private T GetServiceFromDI<T>() where T : notnull
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider is not initialized.");
        }
        return _serviceProvider.GetRequiredService<T>();
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
