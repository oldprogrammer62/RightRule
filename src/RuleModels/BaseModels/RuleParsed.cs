using RightRule.Models.RuleRegistration;
using RightRule.StatementProcessor.Parser;

namespace RightRule.RuleModels.BaseModels;

public abstract class RuleParsed : IRule
{
    protected RuleParsed()
    {
        Node = null;
        StatementPath = string.Empty;
        Message = string.Empty;
    }

    public required int Number { get; init; }

    public required string Name { get; init; }

    public required RuleDefinition RuleDefinition { get; init; }

    public RuleRegistrationStatus Status { get; init; }

    /// <summary>
    /// The Node property represents the parsed rule statement as an abstract syntax tree (AST).
    /// It allows the interpreter to traverse the AST and evaluate the rule statement.
    /// </summary>
    public required Node? Node { get; init; }

    /// <summary>
    /// Provides the AST (Abstract Syntax Tree) path of the statement applied when evaluating the rule.
    /// </summary>
    public required string StatementPath { get; init; }

    /// <summary>
    /// When parsing fails during registration this holds the parser/tokenizer error message.
    /// </summary>
    public string? Message { get; init; }
}
