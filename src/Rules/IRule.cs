namespace RightRule.Rules;

public interface IRule
{
    int Number { get; init; }
    string Name { get; init; }
    string Statement { get; init; }
    string Context { get; init; }
    string Decision { get; init; }
    object? DecisionData { get; init; }
}
