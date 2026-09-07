using Microsoft.Extensions.DependencyInjection;
using RightRule.DataRegistration;
using RightRule.Factories.LanguageFactory;
using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.Operators;
using RightRule.RightRuleService;
using RightRule.RuleRegistration.DefaultRuleRegistration;
using RightRule.RuleRegistration.LogicRules;
using RightRule.StatementProcessor.Evaluator;
using RightRule.StatementProcessor.Interpreter;
using RightRule.StatementProcessor.Parser;
using RightRule.StatementProcessor.Tokenizer;

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
        services.AddScoped<IOperatorsService, OperatorsService>();

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
