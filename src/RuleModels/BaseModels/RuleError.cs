namespace RightRule.RuleModels.BaseModels;

public abstract class RuleError : IRule
{
    public const string DECISIONWORD = "ERROR";
    public const string FALSEDECISIONWORD = "ERROR";

    public string Decision { get; } = DECISIONWORD;
    public object? DecisionData { get; }

    public string FalseDecision { get; } = FALSEDECISIONWORD;
    public object? FalseDecisionData { get; }

    public required string Message { get; init; } = string.Empty;
}
