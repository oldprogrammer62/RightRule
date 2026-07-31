namespace RightRule.Models;

public class RightRuleConfiguration
{
    public bool DiagnosticsEnabled { get; }

    internal RightRuleConfiguration(bool EnableDiagnostics)
    {
        DiagnosticsEnabled = EnableDiagnostics;
    }
}
