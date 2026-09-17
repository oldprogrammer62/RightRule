using Microsoft.Extensions.DependencyInjection;
using RightRule;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RightRuleService;
using RightRule.ServiceRegister;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    private bool Disposed { get; set; }

    private readonly ServiceProvider ServiceProvider;

    private readonly IRightRuleService RightRuleService;

    public RightRule()
    {
        var services = ServiceRegister.RegisterServices(new ServiceCollection());
        ServiceProvider = services.BuildServiceProvider();
        RightRuleService = ServiceProvider.GetRequiredService<IRightRuleService>();
    }

    private List<RightRuleResult<IProcessedRule>> BuildResults(string Context, List<RightRuleServiceResult<IProcessedRule>> serviceResults)
    {
        if (serviceResults == null)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context))];
        }
        if (serviceResults.Count == 0)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context))];
        }
        List<RightRuleResult<IProcessedRule>> rightRuleResults = new List<RightRuleResult<IProcessedRule>>();

        foreach (var serviceResult in serviceResults)
        {
            rightRuleResults.Add(BuildResult(serviceResult));
        }
        return rightRuleResults;
    }

    private RightRuleResult<IProcessedRule> BuildResult(string Context, List<RightRuleServiceResult<IProcessedRule>> serviceResults)
    {
        if (serviceResults == null)
        {
            return RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context));
        }
        if (serviceResults.Count == 0)
        {
            return RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context));
        }
        if (serviceResults.Count == 1)
        {
            return BuildResult(serviceResults[0]);
        }
        return RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context));
    }

    private RightRuleResult<IProcessedRule> BuildResult(RightRuleServiceResult<IProcessedRule> serviceResult)
    {
        var context = serviceResult?.Payload?.Context ?? string.Empty;
        var status = serviceResult?.Status ?? RegistrationStatusEnum.DEFAULT;

        var payload = serviceResult?.Payload ?? null;
        var message = serviceResult?.Message ?? string.Empty;
        var statementApplied = serviceResult?.Payload?.StatementApplied ?? string.Empty;

        return status switch
        {
            RegistrationStatusEnum.SUCCESS when serviceResult?.Payload != null =>
                RightRuleResult<IProcessedRule>.SuccessResult(payload, statementApplied),

            RegistrationStatusEnum.FAILURE =>
                RightRuleResult<IProcessedRule>.FailureResult(message, statementApplied, payload != null ? payload : RightRuleService.GetDefaultRule(context)),

            RegistrationStatusEnum.DEFAULT =>
                RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(context), message),

            RegistrationStatusEnum.ERROR =>
                RightRuleResult<IProcessedRule>.ErrorResult(serviceResult?.Exception ?? new Exception(message), payload != null ? payload : ErrorRule.ErrorResult(message)),

            _ => RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(context))
        };
    }

    public void Dispose()
    {
        if (Disposed) return;

        // Properly disposes of all internal singletons/scoped services
        ServiceProvider.Dispose();
        Disposed = true;
    }
}
