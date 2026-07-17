namespace RightRule.Models;

public class RightRuleConfiguration
{
    public bool DiagnosticsEnabled { get; }
    public bool SilentEvaluatorExceptions { get; }

    public RightRuleConfiguration(bool EnableDiagnostics, bool SilentEvaluatorExceptions)
    {
        DiagnosticsEnabled = EnableDiagnostics;
        this.SilentEvaluatorExceptions = SilentEvaluatorExceptions;
    }
}
