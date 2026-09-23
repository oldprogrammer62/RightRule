using Microsoft.Extensions.DependencyInjection;
using RightRule;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RightRuleService;
using RightRule.ServiceRegister;
using System.ComponentModel;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private bool Disposed { get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly ServiceProvider ServiceProvider;

    private readonly IRightRuleService RightRuleService;

    public IRuleRegistrationService RuleRegistrationService => RightRuleService.RuleRegistrationService;

    public RightRule()
    {
        var services = ServiceRegister.RegisterServices(new ServiceCollection());
        ServiceProvider = services.BuildServiceProvider();
        // Ensure the library-level service locator is configured so extension methods
        // like DefinedRule.Register() can resolve required services without host setup.
        ServiceLocator.SetProvider(ServiceProvider);
        RightRuleService = ServiceProvider.GetRequiredService<IRightRuleService>();
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

        // Properly disposes of all internal singletons/scoped services
        ServiceProvider.Dispose();
        Disposed = true;
    }
}
