using RightRule.Interpreter;
using System.Diagnostics.CodeAnalysis;

namespace RightRule.Rules;

public class ParsedRule : Rule
{
    public required Node? Node { get; init; }

    [SetsRequiredMembers]
    internal ParsedRule(RuleDefinition rule, Node? node)
    {
        Arguments = rule.Arguments;
        Number = rule.Number;
        Name = rule.Name;
        Statement = rule.Statement;
        Context = rule.Context;
        Decision = rule.Decision;
        DecisionData = rule.DecisionData;
        Node = node;
    }
}
