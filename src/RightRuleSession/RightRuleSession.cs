using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RightRuleService;
using RightRule.RuleServices;
using System.Diagnostics;

namespace RightRule.RightRuleSession;

public sealed partial class RightRuleSession : IRightRuleSession
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IRightRuleService _rightRuleService;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly bool _ownsServiceProvider;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)] 
    private int _disposeState;

    public IRuleService RuleService { get; }
    public IDataService DataService { get; }
    public IDataAdaptorService DataAdaptorService { get; }
    public IOperatorService OperatorService { get; }
    public RightRuleOptions RightRuleOptions { get; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IServiceProvider _serviceProvider;
    public IServiceProvider ServiceProvider => _serviceProvider;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly ILogger<RightRuleSession> _logger;
    public ILogger<RightRuleSession> Logger => _logger;

    internal RightRuleSession(IServiceProvider serviceProvider, bool ownsServiceProvider)
    {
        _serviceProvider = serviceProvider;
        _ownsServiceProvider = ownsServiceProvider;

        _rightRuleService = serviceProvider.GetRequiredService<IRightRuleService>();

        RuleService = serviceProvider.GetRequiredService<IRuleService>();
        DataService = serviceProvider.GetRequiredService<IDataService>();
        DataAdaptorService = serviceProvider.GetRequiredService<IDataAdaptorService>();
        OperatorService = serviceProvider.GetRequiredService<IOperatorService>();
        RightRuleOptions = serviceProvider.GetRequiredService<RightRuleOptions>();

        _logger = serviceProvider.GetRequiredService<ILogger<RightRuleSession>>();

        if (_logger.IsEnabled(LogLevel.Debug))
            _logger.LogDebug(
                "RightRule session started. OwnsServiceProvider={OwnsServiceProvider}",
                _ownsServiceProvider);

        LogOptions(_logger, RightRuleOptions);
    }

    private static void LogOptions(
    ILogger<RightRuleSession> logger,
    RightRuleOptions options)
    {
        if (!logger.IsEnabled(LogLevel.Information))
            return;

        logger.LogInformation(
            "Creating RightRule session with options: " +
            "DataCheck={DataCheck}, Diagnostics={Diagnostics}, " +
            "LoadCustomOperators={LoadCustomOperators}, " +
            "LoadDefaultOperators={LoadDefaultOperators}, " +
            "CustomOperatorLibraryPath={CustomOperatorLibraryPath}",
            options.DataCheck,
            options.Diagnostics,
            options.LoadCustomOperators,
            options.LoadDefaultOperators,
            options.CustomOperatorLibraryPath);
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposeState, 1) != 0)
            return;

        if (_ownsServiceProvider && _serviceProvider is IDisposable disposable)
            disposable.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposeState, 1) != 0)
            return;

        if (!_ownsServiceProvider)
            return;

        if (_serviceProvider is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        else if (_serviceProvider is IDisposable disposable)
            disposable.Dispose();
    }
}
