namespace RightRule.Models;

public class RightRuleConfiguration
{
    public bool DiagnosticsEnabled { get; }
    public bool SyntaxCheckWithData { get; }

    internal RightRuleConfiguration(bool EnableDiagnostics, bool SyntaxCheckWithData)
    {
        DiagnosticsEnabled = EnableDiagnostics;
        this.SyntaxCheckWithData = SyntaxCheckWithData;
    }
}
