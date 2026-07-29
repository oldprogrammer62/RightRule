using RightRule.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RightRule.Rules;

/// <summary>
/// Represents a rule that has been applied and evaluated as true against a dataset.
/// </summary>
public sealed class AppliedRule : Rule
{
    /// <summary>
    /// Indicates whether the rule was evaluated as true or false against the dataset. This is useful for logging and debugging purposes, as it allows the calling application to know whether the rule was successfully applied or not. It can also be used to provide additional context when taking action based on the decision.
    /// </summary>
    public required bool Result { get; init; }

    /// <summary>
    /// Used to return a message
    /// - Exception message if an error is thrown
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Diagnostics
    /// Provides the actual statement applied when evaluating the rule. This is useful for logging and debugging purposes, as it allows the calling application to know which statements were applied when making the decision. It can also be used to provide additional context when taking action based on the decision, such as including the statements in a notification or report.
    /// All placeholder arguments will be replaced with their actual values. This allows the calling application to see the exact statement that was evaluated against the dataset, which can be helpful for understanding the decision-making process and for troubleshooting any issues that may arise.
    /// </summary>
    public string StatementApplied { get; init; }

    /// <summary>
    /// Diagnostics
    /// Provides the AST (Abstract Syntax Tree) path of the statement applied when evaluating the rule.
    /// </summary>
    public string StatementPath { get; init; }

    internal static readonly string NoDecision = "NODECISION";

    public AppliedRule() : base()
    {
        StatementApplied = string.Empty;
        StatementPath = string.Empty;
    }

    [SetsRequiredMembers]
    internal AppliedRule(ParsedRule parsedRule, RuleResult result)
        : this()
    {
        if (IsError(result))
        {
            Number = parsedRule.Number;
            Result = false;
            Name = parsedRule.Name;
            Statement = parsedRule.Statement;
            Context = parsedRule.Context;
            Decision = NoDecision;
            DecisionData = string.Empty;
            StatementApplied = result.StatementApplied;
            StatementPath = result.SyntaxTree;
            Message = result.Message;
            return;
        }

        Number = parsedRule.Number;
        Result = result.Result.Value; // Equals(RuleResult.ResultType.ResultTrue);
        Name = parsedRule.Name;
        Statement = parsedRule.Statement;
        Context = parsedRule.Context;
        Decision = parsedRule.Decision;
        DecisionData = parsedRule.DecisionData;
        StatementApplied = result.StatementApplied;
        StatementPath = result.SyntaxTree;
        Message = result.Message;
    }

    private static bool IsError(RuleResult ruleResult)
    {
        return !ruleResult.Result.IsSuccess;
    }

    public static AppliedRule DefaultRule()
    {
        return new AppliedRule()
        {
            Number = -1,
            Result = true,
            Name = "DEFAULTRULE",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = NoDecision,
            DecisionData = string.Empty,
            StatementApplied = string.Empty,
            StatementPath = string.Empty,
            Message = string.Empty
        };
    }

    public static AppliedRule DefaultRule(string message)
    {
        return new AppliedRule()
        {
            Number = -1,
            Result = true,
            Name = "DEFAULTRULE",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = NoDecision,
            DecisionData = string.Empty,
            StatementApplied = string.Empty,
            StatementPath = string.Empty,
            Message = message
        };
    }

    internal static AppliedRule FalseResult(ParsedRule parsedRule, RuleResult result)
    {
        return new AppliedRule()
        {
            Number = parsedRule.Number,
            Result = false,
            Name = parsedRule.Name,
            Statement = parsedRule.Statement,
            Context = parsedRule.Context,
            Decision = NoDecision,
            DecisionData = string.Empty,
            StatementApplied = result.StatementApplied,
            StatementPath = result.SyntaxTree,
            Message = result.Message
        };
    }

    internal static AppliedRule ErrorResult(ParsedRule parsedRule, RuleResult result)
    {
        return new AppliedRule()
        {
            Number = parsedRule.Number,
            Result = false,
            Name = parsedRule.Name,
            Statement = parsedRule.Statement,
            Context = parsedRule.Context,
            Decision = NoDecision,
            DecisionData = string.Empty,
            StatementApplied = result.StatementApplied,
            StatementPath = result.SyntaxTree,
            Message = result.Message
        };
    }

    public override string ToString()
    {
        var statementApplied = !string.IsNullOrWhiteSpace(StatementApplied) ? StatementApplied : "Enable Diagnostics";
        var statementPath = !string.IsNullOrWhiteSpace(StatementPath) ? StatementPath : "Enable Diagnostics";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.Number}");
        sb.AppendLine($"Name: {this.Name}");
        sb.AppendLine($"Context: {this.Context}");
        sb.AppendLine($"Statement: {this.Statement}");
        sb.AppendLine($"Result: {this.Result}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        sb.AppendLine($"StatementApplied: {statementApplied}");
        sb.AppendLine($"StatementPath: {statementPath}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }
}
