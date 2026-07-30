namespace RightRule.Models;

public class RightRuleConfiguration
{
    public bool DiagnosticsEnabled { get; }

    public RightRuleConfiguration(bool EnableDiagnostics)
    {
        DiagnosticsEnabled = EnableDiagnostics;
    }
}
