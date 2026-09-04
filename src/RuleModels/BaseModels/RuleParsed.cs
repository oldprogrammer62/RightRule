using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Parser;

namespace RightRule.RuleModels.BaseModels;

public abstract class RuleParsed : IRule, IParsed
{
    protected RuleParsed()
    {
    }

    public required int Number { get; init; }

    public required string Name { get; init; }

    public required RuleDefinition RuleDefinition { get; init; }

    public required string Context { get; init; }

    public required string Statement { get; init; }

    public required RegistrationStatus Status { get; init; }

    ///// <summary>
    ///// The Node property represents the parsed rule statement as an abstract syntax tree (AST).
    ///// It allows the interpreter to traverse the AST and evaluate the rule statement.
    ///// </summary>
    public required Node? Node { get; init; }

    ///// <summary>
    ///// Provides the AST (Abstract Syntax Tree) path of the statement applied when evaluating the rule.
    ///// </summary>
    public required string StatementPath { get; init; }

    ///// <summary>
    ///// When parsing fails during registration this holds the parser/tokenizer error message.
    ///// </summary>
    public string? Message { get; init; }
}
