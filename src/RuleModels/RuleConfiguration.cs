namespace RightRule.RuleModels;

/// <summary>
/// DataCheck: Optional true/false (default: false) to include a data check during statement parsing
/// Diagnostics: Optional true/false (default: false) provide additional data in rule objects returned to application.
/// </summary>
public class RuleConfiguration
{
    private readonly bool _datacheck = false;
    public bool DataCheck => _datacheck;

    private readonly bool _diagnostics = false;
    public bool Diagnostics => _diagnostics;

    public RuleConfiguration(bool DataCheck, bool Diagnostics)
    {
        _datacheck = DataCheck;
        _diagnostics = Diagnostics;
    }

    public static RuleConfiguration DefaultConfig()
    {
        bool datacheck = false;
        bool diagnostics = false;

        return new RuleConfiguration(datacheck, diagnostics);
    }
}
