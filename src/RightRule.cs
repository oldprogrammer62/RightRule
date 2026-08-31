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

    private static List<RightRuleResult<IRule>> BuildResult(List<RightRuleServiceResult<IRule>> serviceResults)
    {
        List<RightRuleResult<IRule>> rightRuleResults = new List<RightRuleResult<IRule>>();

        foreach (var serviceResult in serviceResults)
        {
            rightRuleResults.Add(BuildResult(serviceResult));
        }
        return rightRuleResults;
    }

    private static RightRuleResult<IRule> BuildResult(RightRuleServiceResult<IRule> serviceResult)
    {
        return serviceResult.Status switch
        {
            ServiceStatus.Success when serviceResult.Payload != null =>
                RightRuleResult<IRule>.Success(serviceResult.Payload),

            ServiceStatus.Failure =>
                RightRuleResult<IRule>.Failure(serviceResult.Message, serviceResult.Payload != null ? serviceResult.Payload : DefaultRule.DefaultResult()),

            ServiceStatus.Default =>
                RightRuleResult<IRule>.Default(DefaultRule.DefaultResult(), serviceResult.Message),

            ServiceStatus.Error =>
                RightRuleResult<IRule>.Error(serviceResult.Exception ?? new Exception(serviceResult.Message), serviceResult.Payload != null ? serviceResult.Payload : ErrorRule.ErrorResult(serviceResult.Message)),

            _ => RightRuleResult<IRule>.Default(DefaultRule.DefaultResult())
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
