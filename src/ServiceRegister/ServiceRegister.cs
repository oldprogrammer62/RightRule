using Microsoft.Extensions.DependencyInjection;
using RightRule.Data;
using RightRule.Operators;
using RightRule.RuleRegistration;
using RightRule.RuleService;

namespace RightRule.ServiceRegister;

internal sealed class ServiceRegister
{
    internal static ServiceCollection RegisterServices(ServiceCollection services)
    {
        if (services is null)
        {
            services = new ServiceCollection();
        }
        services.AddScoped<IRightRuleService, RightRuleService>();
        services.AddScoped<IOperatorsService, OperatorsService>();
        services.AddScoped<IRegisteredRuleService, RegisteredRuleService>();
        services.AddScoped<IRegisteredDatasets, RegisteredDatasets>();
        services.AddScoped<IRegisteredDataObjects, RegisteredDataObjects>();
        services.AddScoped<IRegisteredRules, RegisteredRules>();

        return services;
    }
}
