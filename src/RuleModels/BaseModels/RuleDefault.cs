namespace RightRule.RuleModels.BaseModels;

public abstract class RuleDefault : IRule
{
    public const string DECISIONWORD = "NODECISION";
    public const string FALSEDECISIONWORD = "NODECISION";

    public string Decision { get; } = DECISIONWORD;
    public object? DecisionData { get; init; } = null;

    public string FalseDecision { get; } = FALSEDECISIONWORD;
    public object? FalseDecisionData { get; init; } = null;
}
