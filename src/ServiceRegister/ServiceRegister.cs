using Microsoft.Extensions.DependencyInjection;
using RightRule.Factories.LanguageFactory;
using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RightRuleService;

namespace RightRule.ServiceRegister;

internal sealed class ServiceRegister
{
    internal static ServiceCollection RegisterServices(ServiceCollection services)
    {
        if (services is null)
        {
            services = new ServiceCollection();
        }
        services.AddScoped<IRightRuleService, RightRuleService.RightRuleService>();
        services.AddScoped<IOperatorService, OperatorService>();

        services.AddScoped<IRegisteredRuleService, RegisteredRuleService>();
        services.AddScoped<IRegisteredDatasets, RegisteredDatasets>();
        services.AddScoped<IRegisteredDataObjects, RegisteredDataObjects>();
        services.AddScoped<IRegisteredRules, RegisteredRules>();
        services.AddScoped<IRuleRegistrationService, RuleRegistrationService>();

        services.AddSingleton<IDefaultRuleRegister, DefaultRuleRegister>();

        services.AddScoped<ILanguageFactory, LanguageFactory>();
        services.AddSingleton<IInterpreter, Interpreter>();
        services.AddSingleton<IEvaluator, Evaluator>();
        services.AddSingleton<ITokenizer, Tokenizer>();
        services.AddSingleton<IParser, Parser>();

        return services;
    }
}
