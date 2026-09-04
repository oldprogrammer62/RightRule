namespace RightRule.RuleModels.BaseModels;

public interface IDecideFalse
{
    string FalseDecision { get; }
    object? FalseDecisionData { get; }
}
