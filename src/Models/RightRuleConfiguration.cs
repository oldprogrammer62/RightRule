namespace RightRule.Models;

public class RightRuleConfiguration
{
    public bool DiagnosticsEnabled { get; }
    public bool xSilentEvaluatorExceptions { get; }

    public RightRuleConfiguration(bool EnableDiagnostics, bool xSilentEvaluatorExceptions)
    {
        DiagnosticsEnabled = EnableDiagnostics;
        this.xSilentEvaluatorExceptions = xSilentEvaluatorExceptions;
    }
}
