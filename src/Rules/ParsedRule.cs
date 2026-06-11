using RightRule.Interpreter;
using System.Diagnostics.CodeAnalysis;

namespace RightRule.Rules;

/// <summary>
/// A ParsedRule is a Rule that has been parsed and has an associated Node representing the parsed rule statement.
/// It is used during the evaluation process to evaluate the rule statement against the provided data.
/// The Node property allows the interpreter to traverse the abstract syntax tree (AST) of the parsed rule statement and evaluate it accordingly.
/// This class is internal
/// </summary>
public class ParsedRule : Rule
{
    /// <summary>
    /// The Node property represents the parsed rule statement as an abstract syntax tree (AST).
    /// It allows the interpreter to traverse the AST and evaluate the rule statement accordingly.
    /// </summary>
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
