using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RightRule.DataServices;
using RightRule.RightRuleService;
using RightRule.RuleServices;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleRegistration;
using System.ComponentModel;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private bool Disposed { get; set; }

    private readonly IRightRuleService RightRuleService;

    public IRuleService RuleService => RightRuleService.RuleService ?? throw new InvalidOperationException("Rule Service is not available");

    public IDataService DataService => RightRuleService.DataService ?? throw new InvalidOperationException("Data Service is not available");

    private readonly ILogger<RightRule> _logger;

    private IServiceProvider? _internalServiceProvider;

    public RightRule()
    {
        RightRuleService = GetRightRuleServiceFromDI();
        _logger = new RightRuleLogger.RightRuleLogger<RightRule>();
    }

    public RightRule(IRightRuleService rightRuleService, ILogger<RightRule> logger)
    {
        RightRuleService = rightRuleService ?? throw new ArgumentNullException(nameof(rightRuleService));
        _logger = logger;
    }

    private IRightRuleService GetRightRuleServiceFromDI()
    {
        var services = new ServiceCollection();

        ServiceRegister.ServiceRegister.RegisterServices(services);

        _internalServiceProvider = services.BuildServiceProvider();

        // Resolve required service
        return _internalServiceProvider.GetRequiredService<IRightRuleService>();
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

    private RightRuleResult<IProcessedRule> BuildResult(string Context, List<RightRuleServiceResult<IProcessedRule>> ServiceResults)
    {
        if (ServiceResults == null)
        {
            return RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context));
        }
        if (ServiceResults.Count == 0)
        {
            return RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context));
        }
        if (ServiceResults.Count == 1)
        {
            return BuildResult(ServiceResults[0]);
        }
        return RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context));
    }

    private RightRuleResult<IProcessedRule> BuildResult(RightRuleServiceResult<IProcessedRule> ServiceResult)
    {
        var context = ServiceResult?.Payload?.Context ?? string.Empty;
        var status = ServiceResult?.Status ?? RegistrationStatusEnum.DEFAULT;

        var payload = ServiceResult?.Payload ?? null;
        var message = ServiceResult?.Message ?? string.Empty;
        var statementApplied = ServiceResult?.Payload?.StatementApplied ?? string.Empty;

        return status switch
        {
            RegistrationStatusEnum.SUCCESS when ServiceResult?.Payload != null =>
                RightRuleResult<IProcessedRule>.SuccessResult(payload, statementApplied),

            RegistrationStatusEnum.FAILURE =>
                RightRuleResult<IProcessedRule>.FailureResult(message, statementApplied, payload != null ? payload : RightRuleService.GetDefaultRule(context)),

            RegistrationStatusEnum.DEFAULT =>
                RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(context), message),

            RegistrationStatusEnum.ERROR =>
                RightRuleResult<IProcessedRule>.ErrorResult(ServiceResult?.Exception ?? new Exception(message), payload != null ? payload : ErrorRule.ErrorResult(message)),

            _ => RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(context))
        };
    }

    public void Dispose()
    {
        if (Disposed) return;

        Disposed = true;
    }
}
