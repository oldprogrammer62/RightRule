using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.DataServices.Data;
using RightRule.DataTransformer;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleModels;
using RightRule.System.Common.Operator.Models;
using System.ComponentModel;

namespace RightRule.RightRuleService;

internal sealed partial class RightRuleService : IRightRuleService, IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private bool Disposed { get; set; } = false;

    private readonly IStatementSyntaxChecker StatementSyntaxChecker;
    private readonly IRuleApplicationService RuleApplicationService;
    private readonly IInterpolatorService InterpolatorService;
    private readonly ILogger<RightRuleService> Logger;

    public IDataAdaptorService? DataAdaptorService { get; }
    public IEnumerable<Type>? RegisteredDataProviders => DataAdaptorService?.RegisteredDataAdaptors;

    public IOperatorService? OperatorService { get; }
    public List<Operator>? RegisteredOperators => OperatorService?.Operators;

    private readonly IDefaultRuleRegister DefaultRuleRegister;
    public List<DefaultRulePerContext>? DefaultRules => DefaultRuleRegister.GetDefaultRules();

    public IRuleService RuleService { get; }
    public List<ParsedRule>? RegisteredRules => RuleService?.GetRules();

    public IDataService DataService { get; }
    public List<Dataset>? RegisteredData => DataService?.GetDatasets();

    public RightRuleService(
        IOperatorService operatorsService, 
        IRuleService ruleService, 
        IDefaultRuleRegister defaultRuleRegister, 
        IDataService dataService, 
        IStatementSyntaxChecker statementSyntaxChecker,
        IRuleApplicationService ruleApplicationService,
        IDataAdaptorService dataProviderService,
        IInterpolatorService transformerService,
        ILogger<RightRuleService> logger
        )
    {
        OperatorService = operatorsService;
        RuleService = ruleService;
        DefaultRuleRegister = defaultRuleRegister;
        DataService = dataService;
        StatementSyntaxChecker = statementSyntaxChecker;
        RuleApplicationService = ruleApplicationService;
        DataAdaptorService = dataProviderService;
        InterpolatorService = transformerService;
        Logger = logger;
    }

    public Task<CheckStatementResult> SyntaxCheckAsync(string Statement)
    {
        return StatementSyntaxChecker.CheckAsync(Statement);
    }

    public void Dispose()
    {
        if (Disposed) return;

        if (DataService is IDisposable disposableDataService)
        {
            disposableDataService.Dispose();
        }
        if (RuleService is IDisposable disposableRuleService)
        {
            disposableRuleService.Dispose();
        }
        if (OperatorService is IDisposable disposableOperatorService)
        {
            disposableOperatorService.Dispose();
        }
        Disposed = true;
    }
}
