using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.System.Common.Operator.Models;

namespace RightRule.RightRuleService;

internal sealed partial class RightRuleService : IRightRuleService, IDisposable
{
    private bool Disposed { get; set; } = false;

    private readonly IStatementSyntaxChecker _statementSyntaxChecker;

    public IOperatorService? OperatorService { get; }

    public List<Operator>? RegisteredOperators => OperatorService?.Operators;

    public RightRuleService(
        IOperatorService operatorsService, 
        IStatementSyntaxChecker statementSyntaxChecker
        )
    {
        OperatorService = operatorsService;
        _statementSyntaxChecker = statementSyntaxChecker;
    }

    public Task<CheckStatementResult> SyntaxCheckAsync(string Statement)
    {
        return _statementSyntaxChecker.CheckAsync(Statement);
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
