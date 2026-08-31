namespace RightRule.RuleModels.BaseModels;

public abstract class RuleDefault : IRule
{
    public const string DECISIONWORD = "NODECISION";

    public string Decision { get; } = DECISIONWORD;
    public object? DecisionData { get; init; } = null;
}
