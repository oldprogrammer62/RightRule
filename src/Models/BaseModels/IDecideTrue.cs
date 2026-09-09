namespace RightRule.Models.BaseModels;

public interface IDecideTrue
{
    string Decision { get; }
    object? DecisionData { get; }
}
