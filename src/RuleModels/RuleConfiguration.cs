namespace RightRule.RuleModels;

/// <summary>
/// DataCheck: Optional true/false (default: false) to include a data check during statement parsing
/// Diagnostics: Optional true/false (default: false) provide additional data in rule objects returned to application.
/// </summary>
public class RuleConfiguration
{
    public const bool DEFAULT_DATACHECK = false;
    public const bool DEFAULT_DIAGNOSTICS = true;

    private readonly bool _datacheck = DEFAULT_DATACHECK;
    public bool DataCheck => _datacheck;

    private readonly bool _diagnostics = DEFAULT_DIAGNOSTICS;
    public bool Diagnostics => _diagnostics;

    public RuleConfiguration(bool DataCheck, bool Diagnostics)
    {
        _datacheck = DataCheck;
        _diagnostics = Diagnostics;
    }

    public static RuleConfiguration DefaultConfig()
    {
        bool datacheck = DEFAULT_DATACHECK;
        bool diagnostics = DEFAULT_DIAGNOSTICS;

        return new RuleConfiguration(datacheck, diagnostics);
    }
}
