using Microsoft.Extensions.DependencyInjection;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.RuleService;
using RightRule.ServiceRegister;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    private bool _disposed { get; set; }

    private readonly ServiceProvider _serviceProvider;

    private readonly IRightRuleService _rightRuleService;

    public RightRule()
    {
        var services = ServiceRegister.RegisterServices(new ServiceCollection());
        _serviceProvider = services.BuildServiceProvider();

        _rightRuleService = _serviceProvider.GetRequiredService<IRightRuleService>();
    }

    private List<RightRuleResult<IProcessedRule>> BuildResult(List<RightRuleServiceResult<IProcessedRule>> serviceResults)
    {
        List<RightRuleResult<IProcessedRule>> rightRuleResults = new List<RightRuleResult<IProcessedRule>>();

        foreach (var serviceResult in serviceResults)
        {
            rightRuleResults.Add(BuildResult(serviceResult));
        }
        return rightRuleResults;
    }

    private RightRuleResult<IProcessedRule> BuildResult(RightRuleServiceResult<IProcessedRule> serviceResult)
    {
        return serviceResult.Status switch
        {
            ServiceStatus.Success when serviceResult.Payload != null =>
                RightRuleResult<IProcessedRule>.Success(serviceResult.Payload, serviceResult.Payload.StatementApplied),

            ServiceStatus.Failure =>
                RightRuleResult<IProcessedRule>.Failure(serviceResult.Message, serviceResult?.Payload?.StatementApplied ?? string.Empty, serviceResult?.Payload != null ? serviceResult.Payload : _rightRuleService.GetDefaultRule()),

            ServiceStatus.Default =>
                RightRuleResult<IProcessedRule>.Default(_rightRuleService.GetDefaultRule(), serviceResult.Message),

            ServiceStatus.Error =>
                RightRuleResult<IProcessedRule>.Error(serviceResult.Exception ?? new Exception(serviceResult.Message), serviceResult.Payload != null ? serviceResult.Payload : ErrorRule.ErrorResult(serviceResult.Message)),

            _ => RightRuleResult<IProcessedRule>.Default(_rightRuleService.GetDefaultRule())
        };
    }

    public void Dispose()
    {
        if (_disposed) return;

        // Properly disposes of all internal singletons/scoped services
        _serviceProvider.Dispose();
        _disposed = true;
    }
}
