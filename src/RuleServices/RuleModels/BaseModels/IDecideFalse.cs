namespace RightRule.RuleServices.RuleModels.BaseModels;

public interface IDecideFalse
{
    string FalseDecision { get; }
    object? FalseDecisionData { get; }
}
