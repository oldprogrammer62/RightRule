namespace RightRule.RuleModels.BaseModels;

public interface IDecideTrue
{
    string Decision { get; }
    object? DecisionData { get; }
}
