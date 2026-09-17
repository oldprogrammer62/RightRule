using Microsoft.Extensions.DependencyInjection;
using RightRule.Factories.ParserFactory;
using RightRule.Factories.TokenizerFactory;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RegistrationServices.RuleRegistration.Rules;
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
        services.AddSingleton<IRightRuleService, RightRuleService.RightRuleService>();
        services.AddSingleton<IOperatorService, OperatorService>();

        services.AddSingleton<ITokenizerFactory, TokenizerFactory>();
        services.AddSingleton<IParserFactory, ParserFactory>();

        services.AddScoped<IInterpreter, Interpreter>();
        services.AddScoped<IEvaluator, Evaluator>();
        services.AddTransient<IParser, Parser>();
        services.AddTransient<ITokenizer, Tokenizer>();

        services.AddSingleton<IDataService, DataService>();

        services.AddSingleton<IRuleService, RuleService>();
        services.AddScoped<IRuleRegistrationService, RuleRegistrationService>();
        services.AddSingleton<IRegisteredRules, RegisteredRules>();
        services.AddSingleton<IDefaultRuleRegister, DefaultRuleRegister>();

        services.AddScoped<IStatementBuilder, StatementBuilder>();
        services.AddScoped<IStatementSyntaxChecker, StatementSyntaxChecker>();

        return services;
    }
}
