using System.Diagnostics.CodeAnalysis;

namespace RightRule.Rules;

public class RegisteredRule
{
    public required int Number { get; init; }
    public required string Name { get; init; }
    public required string Statement { get; init; }
    public required string Context { get; init; }
    public required string Decision { get; init; }
    public required object? DecisionData { get; init; }

    public RegisteredRule()
    {
    }

    [SetsRequiredMembers]
    public RegisteredRule(ParsedRule rule)
    {
        Number = rule.Number;
        Name = rule.Name;
        Statement = rule.Statement;
        Context = rule.Context;
        Decision = rule.Decision;
        DecisionData = rule.DecisionData;
    }
}
