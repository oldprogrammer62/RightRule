using RightRule.LanguageProcessor.Parser;
using RightRule.Models;

namespace RightRule.RuleServices.RuleModels.BaseModels;

public interface IParsed
{
    RightRuleStatusEnum Status { get; init; }

    /// <summary>
    /// The Node property represents the parsed rule statement as an abstract syntax tree (AST).
    /// It allows the interpreter to traverse the AST and evaluate the rule statement.
    /// </summary>
    Node? Node { get; init; }

    string Statement { get; init; }

    /// <summary>
    /// Provides the AST (Abstract Syntax Tree) path of the statement applied when evaluating the rule.
    /// </summary>
    string StatementPath { get; init; }

    /// <summary>
    /// When parsing fails during registration this holds the parser/tokenizer error message.
    /// </summary>
    string? Message { get; init; }
}
