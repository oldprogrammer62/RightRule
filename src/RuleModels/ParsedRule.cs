using RightRule.RuleModels.BaseModels;
using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Parser;
using System.Text;

namespace RightRule.RuleModels;

/// <summary>
/// A ParsedRule is a Rule that has been parsed and has an associated Node representing the parsed rule statement.
/// It is used during the evaluation process to evaluate the rule statement against the provided data.
/// The Node property allows the interpreter to traverse the abstract syntax tree (AST) of the parsed rule statement and evaluate it accordingly.
/// This class is internal
/// </summary>
public sealed class ParsedRule
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

    /// <summary>
    /// When parsing fails during registration this holds the parser/tokenizer error message.
    /// </summary>
    public string? Message { get; init; }

    private ParsedRule() : base()
    {
        Node = null;
        StatementPath = string.Empty;
        Message = string.Empty;
    }

    internal static ParsedRule? SuccessResult(DefinedRule? ruleDefinition, Node node, string message)
    {
        if (ruleDefinition == null)
        {
            return null;
        }

        return new ParsedRule()
        {
            RuleDefinition = ruleDefinition,
            Message = message,
            StatementPath = BuildTreeText(ruleDefinition, node),
            Node = node
        };
    }

    internal static ParsedRule? ErrorResult(DefinedRule? ruleDefinition, string message)
    {
        if (ruleDefinition == null)
        {
            return null;
        }

        return new ParsedRule()
        {
            RuleDefinition = ruleDefinition,
            Message = message,
            StatementPath = string.Empty,
            Node = null
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.RuleDefinition.Number}");
        sb.AppendLine($"Name: {this.RuleDefinition.Name}");
        sb.AppendLine($"Context: {this.RuleDefinition.Context}");
        sb.AppendLine($"Statement: {this.RuleDefinition.Statement}");
        sb.AppendLine($"Configuration: {this.RuleDefinition.OptionalData}");
        sb.AppendLine($"Decision: {this.RuleDefinition.Decision}");
        sb.AppendLine($"DecisionData: {this.RuleDefinition.DecisionData}");
        sb.AppendLine($"OptionalData: {this.RuleDefinition.OptionalData}");
        sb.AppendLine($"StatementPath: {this.StatementPath}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }

    private static string BuildTreeText(DefinedRule? ruleDefinition, Node node)
    {
        bool diagnostics = Helper.GetDiagnosticsSetting(ruleDefinition); ;
        if (diagnostics)
        {
            return StatementProcessor.StatementPathBuilder.BuildTreeText(node);
        }
        return string.Empty;
    }
}
