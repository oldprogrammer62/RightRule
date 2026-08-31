using RightRule.StatementProcessor.Parser;

namespace RightRule.RuleModels.BaseModels;

internal abstract class RuleParsed
{
    public required RuleDefinition RuleDefinition { get; init; }

    /// <summary>
    /// The Node property represents the parsed rule statement as an abstract syntax tree (AST).
    /// It allows the interpreter to traverse the AST and evaluate the rule statement.
    /// </summary>
    public required Node? Node { get; init; }

    /// <summary>
    /// Provides the AST (Abstract Syntax Tree) path of the statement applied when evaluating the rule.
    /// </summary>
    public required string StatementPath { get; init; }
}
