namespace RightRule.RuleServices.RuleModels.BaseModels;

public abstract class RuleError : IProcessedRule
{
    public const string DECISIONWORD = "ERROR";
    public const string FALSEDECISIONWORD = "ERROR";
    public const string RULENAME = "ErrorRule";

    public int Number { get; init; } = (int)RuleNumberEnum.ERRORRULE;
    public string Name { get; init; } = RULENAME;
    public string Context { get; init; } = string.Empty;

    public bool Success { get; } = false;
    public bool Result { get; } = false;
    public string StatementApplied { get; init; } = string.Empty;
    public required string Message { get; init; } = string.Empty;

    public string TrueDecision { get; } = DECISIONWORD;
    public object? TrueDecisionData { get; }

    public string FalseDecision { get; } = FALSEDECISIONWORD;
    public object? FalseDecisionData { get; }
}
