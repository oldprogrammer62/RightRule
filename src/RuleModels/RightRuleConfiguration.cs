namespace RightRule.RuleModels; 

/// <summary>
/// DataCheck: Optional true/false (default: false) to include a data check during statement parsing
/// Diagnostics: Optional true/false (default: false) provide additional data in rule objects returned to application.
/// </summary>
public class RightRuleConfiguration
{
    public const bool DEFAULT_DATACHECK = false;
    public const bool DEFAULT_DIAGNOSTICS = true;

    public bool DataCheck { get; init; } = DEFAULT_DATACHECK;

    public bool Diagnostics { get; init; } = DEFAULT_DIAGNOSTICS;

    public RightRuleConfiguration()
    {
        DataCheck = DEFAULT_DATACHECK;
        Diagnostics = DEFAULT_DIAGNOSTICS;
    }

    public RightRuleConfiguration(bool dataCheck, bool diagnostics)
    {
        DataCheck = dataCheck;
        Diagnostics = diagnostics;
    }

    public static RightRuleConfiguration DefaultConfig()
    {
        bool datacheck = DEFAULT_DATACHECK;
        bool diagnostics = DEFAULT_DIAGNOSTICS;

        return new RightRuleConfiguration(datacheck, diagnostics);
    }
}
