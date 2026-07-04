using System.Collections.Generic;
using System.Linq;

namespace RightRule.StatementProcessor;

public sealed class CheckStatementResult
{
    public IReadOnlyList<Diagnostic> Diagnostics { get; }

    public CheckStatementResult(IEnumerable<Diagnostic>? diagnostics = null)
    {
        Diagnostics = diagnostics?.ToList().AsReadOnly() ?? new List<Diagnostic>().AsReadOnly();
    }

    public bool IsValid => !Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);

    public static CheckStatementResult Success() => new CheckStatementResult(null);
}