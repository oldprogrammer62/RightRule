using RightRule.Models;
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
public sealed class ParsedRule : Rule
{
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

    /// <summary>
    /// Configuration details
    /// </summary>
    public RightRuleConfiguration RuleConfiguration { get; init; } = new RightRuleConfiguration();

    /// <summary>
    /// Optional data that will be passed to the Operator when evaluating the Rule.
    /// It is the responsibility of the Operator to validate the configuration details and use it as required.
    /// </summary>
    public object? OptionalData { get; init; }

    private ParsedRule() : base()
    {
        Node = null;
        StatementPath = string.Empty;
        Message = string.Empty;
    }

    internal static ParsedRule? SuccessResult(RuleDefinition? ruleDefinition, Node node, string message)
    {
        if (ruleDefinition == null)
        {
            return null;
        }

        return new ParsedRule()
        {
            Message = message,
            Context = ruleDefinition.Context,
            Decision = ruleDefinition.Decision,
            DecisionData = ruleDefinition.DecisionData,
            Name = ruleDefinition.Name,
            Number = ruleDefinition.Number,
            Statement = ruleDefinition.Statement,
            StatementPath = BuildTreeText(ruleDefinition, node),
            OptionalData = ruleDefinition.OptionalData,
            RuleConfiguration = ruleDefinition.RuleConfiguration,
            Node = node
        };
    }

    internal static ParsedRule? ErrorResult(RuleDefinition? ruleDefinition, string message)
    {
        if (ruleDefinition == null)
        {
            return null;
        }

        return new ParsedRule()
        {
            Message = message,
            Context = ruleDefinition.Context,
            Decision = ruleDefinition.Decision,
            DecisionData = ruleDefinition.DecisionData,
            Name = ruleDefinition.Name,
            Number = ruleDefinition.Number,
            Statement = ruleDefinition.Statement,
            StatementPath = string.Empty,
            OptionalData = ruleDefinition.OptionalData,
            RuleConfiguration = ruleDefinition.RuleConfiguration,
            Node = null
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.Number}");
        sb.AppendLine($"Name: {this.Name}");
        sb.AppendLine($"Context: {this.Context}");
        sb.AppendLine($"Statement: {this.Statement}");
        sb.AppendLine($"Configuration: {this.OptionalData}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        sb.AppendLine($"OptionalData: {this.OptionalData}");
        sb.AppendLine($"StatementPath: {this.StatementPath}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }

    private static string BuildTreeText(RuleDefinition? ruleDefinition, Node node)
    {
        bool diagnostics = Helper.GetDiagnosticsSetting(ruleDefinition); ;
        if (diagnostics)
        {
            return StatementProcessor.StatementPathBuilder.BuildTreeText(node);
        }
        return string.Empty;
    }
}
