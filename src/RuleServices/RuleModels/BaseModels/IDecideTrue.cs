namespace RightRule.RuleServices.RuleModels.BaseModels;

public interface IDecideTrue
{
    string TrueDecision { get; }
    object? TrueDecisionData { get; }
}
