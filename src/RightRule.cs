using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RightRule.DataServices;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RightRuleService;
using RightRule.RuleServices;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
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

    private readonly IRightRuleService RightRuleService;

    public IRuleService RuleService => RightRuleService.RuleService ?? throw new InvalidOperationException("Rule Service is not available");

    public IDataService DataService => RightRuleService.DataService ?? throw new InvalidOperationException("Data Service is not available");

    public IOperatorService OperatorService => RightRuleService.OperatorService ?? throw new InvalidOperationException("Operator Service is not available");

    private readonly ILogger<RightRule> Logger;

    private readonly IServiceProvider? ServiceProvider;
    private readonly RightRuleOptions _options;

    /// <summary>
    /// Parameterless constructor
    /// Called by: new RightRule();
    /// Service Provider DI Container is hosted by RightRule
    /// Service Provider DI Container will be disposed when RightRule is disposed
    /// </summary>
    public RightRule() : 
        this(new RightRuleOptions())
    {
    }

    public RightRule(RightRuleOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options;

        ServiceProvider = InitialiseServiceProvider(options);
        RightRuleService = GetRightRuleServiceFromDI();
        Logger = new RightRuleLogger.RightRuleLogger<RightRule>();
    }

    /// <summary>
    /// Called by DI container hosted by calling application
    /// </summary>
    /// <param name = "rightRuleService" ></param >
    /// <param name="logger"></param>
    public RightRule(IRightRuleService rightRuleService, ILogger<RightRule> logger)
    {
        RightRuleService = rightRuleService;
        Logger = logger;
    }

    public RightRuleOptions GetRightRuleOptions()
    {
        return _options;
    }

    private ServiceProvider InitialiseServiceProvider(RightRuleOptions options)
    {
        var services = new ServiceCollection();
        services.AddSingleton(options);
        ServiceRegister.ServiceRegister.RegisterServices(services);
        return services.BuildServiceProvider();
    }

    private IRightRuleService GetRightRuleServiceFromDI()
    {
        if (ServiceProvider == null)
        {
            throw new InvalidOperationException("Service provider is not initialized.");
        }
        return ServiceProvider.GetRequiredService<IRightRuleService>();
    }

    private List<RightRuleResult<IProcessedRule>> BuildResults(string Context, List<RightRuleServiceResult<IProcessedRule>> ServiceResults)
    {
        if (ServiceResults == null)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context))];
        }
        if (ServiceResults.Count == 0)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context))];
        }
        List<RightRuleResult<IProcessedRule>> rightRuleResults = new List<RightRuleResult<IProcessedRule>>();

        foreach (var serviceResult in ServiceResults)
        {
            rightRuleResults.Add(BuildResult(serviceResult));
        }
        return rightRuleResults;
    }

    private RightRuleResult<IProcessedRule> BuildResult(RightRuleServiceResult<IProcessedRule> ServiceResult)
    {
        var context = ServiceResult?.Payload?.Context ?? string.Empty;
        var status = ServiceResult?.Status ?? RightRuleStatusEnum.DEFAULT;

        var payload = ServiceResult?.Payload ?? null;
        var message = ServiceResult?.Message ?? string.Empty;
        var statementApplied = ServiceResult?.Payload?.StatementApplied ?? string.Empty;

        return status switch
        {
            RightRuleStatusEnum.SUCCESS when ServiceResult?.Payload != null =>
                RightRuleResult<IProcessedRule>.SuccessResult(payload, statementApplied),

            RightRuleStatusEnum.FAILURE =>
                RightRuleResult<IProcessedRule>.FailureResult(message, statementApplied, payload != null ? payload : RightRuleService.GetDefaultRule(context)),

            RightRuleStatusEnum.DEFAULT =>
                RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(context), message),

            RightRuleStatusEnum.ERROR =>
                RightRuleResult<IProcessedRule>.ErrorResult(ServiceResult?.Exception ?? new Exception(message), payload != null ? payload : ErrorRule.ErrorResult(message)),

            _ => RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(context))
        };
    }

    public void Dispose()
    {
        if (Disposed) return;

        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
        if (RightRuleService is IDisposable disposableRightRuleService)
        {
            disposableRightRuleService.Dispose();
        }
        Disposed = true;
    }
}
