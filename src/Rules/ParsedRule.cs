using RightRule.Interpreter;

namespace RightRule.Rules;

public class ParsedRule
{
    public required int Number { get; init; }
    public required string Name { get; init; }
    public required string Context { get; init; }
    public required string Decision { get; init; }
    public required object? DecisionData { get; init; }
    public required Node? Node { get; init; }

    public static ParsedRule DefaultRule()
    {
        return new ParsedRule()
        {
            Number = -1,
            Name = "DefaultRule",
            Context = string.Empty,
            Decision = "NODECISION",
            DecisionData = null,
            Node = null
        };
    }
}
