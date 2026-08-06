namespace RightRule.SyntaxChecker;

public sealed class CheckStatementResult
{
    public IReadOnlyList<Diagnostic> Diagnostics { get; }

    public CheckStatementResult(IEnumerable<Diagnostic>? diagnostics = null)
    {
        Diagnostics = diagnostics?.ToList().AsReadOnly() ?? new List<Diagnostic>().AsReadOnly();
    }

    public bool IsValid => !Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);

    public static CheckStatementResult Success() => new CheckStatementResult(null);
    public static CheckStatementResult Failure()
    {
        return new CheckStatementResult(new List<Diagnostic>
        {
            new Diagnostic("Syntax error", DiagnosticSeverity.Error)
        });
    }
}