using Microsoft.Extensions.DependencyInjection;
using RightRule.Data;
using RightRule.Factories.LanguageFactory;
using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.OperatorServices;
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

        services.AddSingleton<IDataService, DataService>();
        services.AddScoped<IStatementBuilder, StatementBuilder>();


        services.AddScoped<IRuleService, RuleService>();
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
