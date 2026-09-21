namespace RightRule.LanguageProcessor.SyntaxChecker;

public sealed class Diagnostic
{
    public Diagnostic()
    {
    }

    public Diagnostic(string Message, DiagnosticSeverityEnum diagnosticSeverity)
    {
        this.Message = Message;
        this.Severity = diagnosticSeverity;
    }

    public string Message { get; init; } = string.Empty;
    public DiagnosticSeverityEnum Severity { get; init; } = DiagnosticSeverityEnum.ERROR;
    public string? Code { get; init; }
    public int Position { get; init; } = -1;   // absolute position in the statement
    public int Length { get; init; } = 0;      // token length at Position
    public string? Snippet { get; init; }      // small context window around the token
    public string? Suggestion { get; init; }

    public override string ToString()
        => $"{Severity}: {Message} (pos: {Position}, len: {Length})" + (string.IsNullOrEmpty(Code) ? string.Empty : $" [{Code}]");
}