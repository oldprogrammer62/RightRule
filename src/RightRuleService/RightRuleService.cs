using RightRule.LanguageProcessor.SyntaxChecker;

namespace RightRule.RightRuleService;

internal sealed partial class RightRuleService : IRightRuleService, IDisposable
{
    private bool Disposed { get; set; } = false;

    private readonly IStatementSyntaxChecker _statementSyntaxChecker;

    public RightRuleService(IStatementSyntaxChecker statementSyntaxChecker
        )
    {
        _statementSyntaxChecker = statementSyntaxChecker;
    }

    public Task<CheckStatementResult> SyntaxCheckAsync(string Statement)
    {
        return _statementSyntaxChecker.CheckAsync(Statement);
    }

    public void Dispose()
    {
        if (Disposed) return;

        if (_statementSyntaxChecker is IDisposable disposableSyntaxCheckerService)
        {
            disposableSyntaxCheckerService.Dispose();
        }

        Disposed = true;
    }
}
