using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.DataTransformer;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.RuleApplication;
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

    public IDataAdaptorService? DataAdaptorService { get; }
    public IEnumerable<Type>? RegisteredDataProviders => DataAdaptorService?.RegisteredDataAdaptors;

    public IOperatorService? OperatorService { get; }

    public List<Operator>? RegisteredOperators => OperatorService?.Operators;

    public IRuleService RuleService { get; }
    
    public IDataService DataService { get; }

    public RightRuleService(
        IOperatorService operatorsService, 
        IRuleService ruleService, 
        IDataService dataService, 
        IStatementSyntaxChecker statementSyntaxChecker,
        IRuleApplicationService ruleApplicationService,
        IDataAdaptorService dataProviderService,
        IInterpolatorService transformerService
        )
    {
        OperatorService = operatorsService;
        RuleService = ruleService;
        DataService = dataService;
        StatementSyntaxChecker = statementSyntaxChecker;
        RuleApplicationService = ruleApplicationService;
        DataAdaptorService = dataProviderService;
        InterpolatorService = transformerService;
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
