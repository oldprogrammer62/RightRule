using Microsoft.Extensions.DependencyInjection;
using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.Factories.ParserFactory;
using RightRule.Factories.TokenizerFactory;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.OperatorServices;
using RightRule.RightRuleService;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleParsingServices;
using RightRule.RuleServices.Rules;

namespace RightRule.ServiceRegister;

internal sealed class ServiceRegister
{
    internal static ServiceCollection RegisterServices(ServiceCollection services)
    {
        if (services is null)
        {
            services = new ServiceCollection();
        }

        services.AddSingleton<IOperatorService, OperatorService>();

        services.AddSingleton<ITokenizerFactory, TokenizerFactory>();
        services.AddSingleton<IParserFactory, ParserFactory>();

        services.AddScoped<IInterpreter, Interpreter>();
        services.AddScoped<IEvaluator, Evaluator>();
        services.AddTransient<IParser, Parser>();
        services.AddTransient<ITokenizer, Tokenizer>();

        services.AddSingleton<IDataService, DataService>();

        services.AddSingleton<IRuleService, RuleService>();
        services.AddScoped<IParseRuleService, ParseRuleService>();
        services.AddSingleton<IRegisteredRules, RegisteredRules>();

        services.AddSingleton<IDefaultRuleRegister, DefaultRuleRegister>();

        services.AddScoped<IStatementBuilder, StatementBuilder>();
        services.AddScoped<IStatementSyntaxChecker, StatementSyntaxChecker>();

        services.AddScoped<IDataValidatorService, DataValidatorService>();

        services.AddScoped<IRuleApplicationService, RuleApplicationService>();
        services.AddScoped<IApplyFirstRuleService, ApplyFirstRuleService>();
        services.AddScoped<IApplyAnyRulesService, ApplyAnyRulesService>();
        services.AddScoped<IApplyAllRulesService, ApplyAllRulesService>();

        services.AddSingleton<IRightRuleService, RightRuleService.RightRuleService>();
        services.AddScoped<IRightRule, RightRule>();

        return services;
    }
}
