namespace RightRule.Models.BaseModels;

public interface IDecideFalse
{
    string FalseDecision { get; }
    object? FalseDecisionData { get; }
}
