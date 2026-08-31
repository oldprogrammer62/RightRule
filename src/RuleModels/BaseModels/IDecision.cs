namespace RightRule.RuleModels.BaseModels;

public interface IDecision
{
    string Decision { get; }
    object? DecisionData { get; }
}