using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RightRule.DataProvider;
using RightRule.DataProvider.DataProviderLibrary;
using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.DataTransformer;
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
    internal static IServiceCollection RegisterServices(IServiceCollection services)
    {
        if (services is null)
        {
            services = new ServiceCollection();
        }

        services.AddLogging();

        services.AddScoped<IRightRule>(sp =>
            new RightRule(
        sp.GetRequiredService<IRightRuleService>(),
        sp.GetRequiredService<ILogger<RightRule>>()));

        services.AddScoped<IRightRuleService, RightRuleService.RightRuleService>();
        services.AddScoped<ITokenizerFactory, TokenizerFactory>();
        services.AddScoped<IParserFactory, ParserFactory>();
        services.AddScoped<IInterpreter, Interpreter>();
        services.AddScoped<IEvaluator, Evaluator>();
        services.AddScoped<IParser, Parser>();
        services.AddScoped<ITokenizer, Tokenizer>();
        services.AddScoped<IParseRuleService, ParseRuleService>();
        services.AddScoped<IStatementBuilder, StatementBuilder>();
        services.AddScoped<IStatementSyntaxChecker, StatementSyntaxChecker>();
        services.AddScoped<IRuleApplicationService, RuleApplicationService>();
        services.AddScoped<IApplyFirstRuleService, ApplyFirstRuleService>();
        services.AddScoped<IApplyAnyRulesService, ApplyAnyRulesService>();
        services.AddScoped<IApplyAllRulesService, ApplyAllRulesService>();

        services.AddScoped<IDataService, DataService>();
        services.AddScoped<IRuleService, RuleService>();
        services.AddScoped<IOperatorService, OperatorService>();
        services.AddScoped<IRegisteredRules, RegisteredRules>();
        services.AddScoped<IDefaultRuleRegister, DefaultRuleRegister>();
        services.AddScoped<IDataProviderLibraryManager, DataProviderLibraryManager>();
        services.AddScoped<IDataProviderService, DataProviderService>();

        services.AddScoped<IInterpolatorService, InterpolatorService>();
        services.AddScoped<IInterpolate, StringInterpolator>();

        services.AddScoped<IDataValidatorService, DataValidatorService>();

        return services;
    }
}
