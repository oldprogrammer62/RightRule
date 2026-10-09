namespace RightRule.ConfigurationService; 

/// <summary>
/// Diagnostics: Optional true/false (default: false) provide additional data in rule objects returned to application.
/// </summary>
public class RightRuleConfiguration
{
    public const bool DEFAULT_DIAGNOSTICS = true;

    public bool Diagnostics { get; init; } = DEFAULT_DIAGNOSTICS;

    public RightRuleConfiguration()
    {
        Diagnostics = DEFAULT_DIAGNOSTICS;
    }

    public RightRuleConfiguration(bool diagnostics)
    {
        Diagnostics = diagnostics;
    }

    public static RightRuleConfiguration DefaultConfig()
    {
        bool diagnostics = DEFAULT_DIAGNOSTICS;

        return new RightRuleConfiguration(diagnostics);
    }
}
