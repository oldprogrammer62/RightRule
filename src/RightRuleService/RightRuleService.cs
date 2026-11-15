using RightRule.DataAdaptors;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.System.Common.Operator.Models;
using System.ComponentModel;

namespace RightRule.RightRuleService;

internal sealed partial class RightRuleService : IRightRuleService, IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private bool Disposed { get; set; } = false;

    private readonly IStatementSyntaxChecker StatementSyntaxChecker;

    public IDataAdaptorService? DataAdaptorService { get; }
    public IEnumerable<Type>? RegisteredDataProviders => DataAdaptorService?.RegisteredDataAdaptors;

    public IOperatorService? OperatorService { get; }

    public List<Operator>? RegisteredOperators => OperatorService?.Operators;

    public RightRuleService(
        IOperatorService operatorsService, 
        IStatementSyntaxChecker statementSyntaxChecker,
        IDataAdaptorService dataProviderService
        )
    {
        OperatorService = operatorsService;
        StatementSyntaxChecker = statementSyntaxChecker;
        DataAdaptorService = dataProviderService;
    }

    public Task<CheckStatementResult> SyntaxCheckAsync(string Statement)
    {
        return StatementSyntaxChecker.CheckAsync(Statement);
    }

    public void Dispose()
    {
        if (Disposed) return;

        if (OperatorService is IDisposable disposableOperatorService)
        {
            disposableOperatorService.Dispose();
        }
        Disposed = true;
    }
}
