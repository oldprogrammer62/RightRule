using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors;
using RightRule.DataAdaptors.DataAdaptorLibrary;
using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.DataTransformer;
using RightRule.Factories.ParserFactory;
using RightRule.Factories.RuleLoaderFactory;
using RightRule.Factories.TokenizerFactory;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RightRuleService;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleLoader;
using RightRule.RuleServices.RuleParsingService;
using RightRule.RuleServices.Rules;
using RightRule.RuleServices.RuleValidation;

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
            new RightRule
            (
                sp.GetRequiredService<IRightRuleService>(),
                sp.GetRequiredService<ILogger<RightRule>>(),
                sp.GetRequiredService<RightRuleOptions>()
            )
        );

        services.AddScoped<IRightRuleService, RightRuleService.RightRuleService>();

        services.AddScoped<ITokenizerFactory, TokenizerFactory>();
        services.AddScoped<ITokenizer, Tokenizer>();

        services.AddScoped<IParserFactory, ParserFactory>();
        services.AddScoped<IParser, Parser>();
        services.AddScoped<IParseRuleService, ParseRuleService>();

        services.AddScoped<IInterpreter, Interpreter>();
        services.AddScoped<IEvaluator, Evaluator>();

        services.AddScoped<IStatementBuilder, StatementBuilder>();
        services.AddScoped<IStatementSyntaxChecker, StatementSyntaxChecker>();

        services.AddScoped<IRuleService, RuleService>();
        services.AddScoped<IRegisteredRules, RegisteredRules>();
        services.AddScoped<IDefaultRuleRegister, DefaultRuleRegister>();
        services.AddScoped<IRuleLoaderFactory, RuleLoaderFactory>();
        services.AddScoped<IRuleLoaderService, RuleLoaderService>();
        services.AddScoped<IRuleApplicationService, RuleApplicationService>();
        services.AddScoped<IRuleValidationService, RuleValidationService>();
        services.AddScoped<IApplyFirstRuleService, ApplyFirstRuleService>();
        services.AddScoped<IApplyAnyRulesService, ApplyAnyRulesService>();
        services.AddScoped<IApplyAllRulesService, ApplyAllRulesService>();

        services.AddScoped<IOperatorService, OperatorService>();

        services.AddScoped<IDataService, DataService>();
        services.AddScoped<IDataAdaptorLibraryManager, DataAdaptorLibraryManager>();
        services.AddScoped<IDataAdaptorService, DataAdaptorService>();
        services.AddScoped<IDataValidatorService, DataValidatorService>();

        services.AddScoped<IInterpolatorService, InterpolatorService>();
        services.AddScoped<IInterpolate, StringInterpolator>();

        return services;
    }
}
