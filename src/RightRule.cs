using Microsoft.Extensions.DependencyInjection;
using RightRule.RuleService;
using RightRule.ServiceRegister;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    private bool _disposed;
        
    private readonly ServiceProvider _serviceProvider;

    private readonly IRightRuleService _rightRuleService;
    internal IRightRuleService RightRuleService => _rightRuleService;

    public RightRule()
    {
        var services = ServiceRegister.RegisterServices(new ServiceCollection());
        _serviceProvider = services.BuildServiceProvider();

        _rightRuleService = _serviceProvider.GetRequiredService<IRightRuleService>();
    }

    public void Dispose()
    {
        if (_disposed) return;

        // Properly disposes of all internal singletons/scoped services
        _serviceProvider.Dispose();
        _disposed = true;
    }
}
