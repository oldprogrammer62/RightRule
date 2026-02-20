namespace RightRule.Rules;

public class Rule : IRule
{
    public required int Number { get; init; }
    public required string Name { get; init; }
    public required string Statement { get; init; }
    public required string Context { get; init; }
    public required string Decision { get; init; }
    public object? DecisionData { get; init; }
}
