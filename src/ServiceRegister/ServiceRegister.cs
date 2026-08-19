using Microsoft.Extensions.DependencyInjection;
using RightRule.Data;
using RightRule.Factories.RightFactory;
using RightRule.Operators;
using RightRule.RuleRegistration;
using RightRule.RuleService;
using RightRule.StatementProcessor.Evaluator;

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

        services.AddScoped<IRightRuleFactory, RightRuleFactory>();
        services.AddSingleton<IEvaluator, Evaluator>();

        return services;
    }
}
