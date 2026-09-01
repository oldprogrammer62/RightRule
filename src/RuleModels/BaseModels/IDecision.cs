namespace RightRule.RuleModels.BaseModels;

public interface IDecision
{
    bool IsSuccess { get; }
    bool Result { get; }

    string StatementApplied { get; }

    string Decision { get; }
    object? DecisionData { get; }

    string FalseDecision { get; }
    object? FalseDecisionData { get; }
}