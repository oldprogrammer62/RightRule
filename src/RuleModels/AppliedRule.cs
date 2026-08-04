using RightRule.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RightRule.RuleModels;

/// <summary>
/// Represents a rule that has been applied and evaluated as true against a dataset.
/// </summary>
public sealed class AppliedRule : Rule
{
    /// <summary>
    /// Indicates whether the Rule logic evaluated as true or false against the dataset.
    /// </summary>
    public required bool Result { get; init; }

    /// <summary>
    /// Used to return a message
    /// - Exception message if an error is thrown
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Diagnostics only
    /// Provides the actual statement applied when evaluating the rule.
    /// All placeholder arguments will be replaced with their actual values.
    /// </summary>
    public string StatementApplied { get; init; }

    /// <summary>
    /// Diagnostics only
    /// Provides the AST (Abstract Syntax Tree) path of the statement applied when evaluating the rule.
    /// </summary>
    public string StatementPath { get; init; }

    /// <summary>
    /// Decision returned when no Rule evaluates to true for the specified Context.
    /// </summary>
    public static string DefaultDecision { get; } = "NODECISION";

    /// <summary>
    /// Decision returned when an error occurs during Rule evaluation.
    /// </summary>
    public static string ErrorDecision { get; } = "ERROR";

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
            Decision = ErrorDecision;
            DecisionData = string.Empty;
            OperatorConfig = parsedRule.OperatorConfig;
            StatementApplied = result.StatementApplied;
            StatementPath = result.SyntaxTree;
            Message = result.Message;
            return;
        }

        Number = parsedRule.Number;
        Result = result.Result.Value;
        Name = parsedRule.Name;
        Statement = parsedRule.Statement;
        Context = parsedRule.Context;
        Decision = parsedRule.Decision;
        DecisionData = parsedRule.DecisionData;
        OperatorConfig = parsedRule.OperatorConfig;
        StatementApplied = result.StatementApplied;
        StatementPath = result.SyntaxTree;
        Message = result.Message;
    }

    internal static AppliedRule DefaultResult()
    {
        return new AppliedRule()
        {
            Number = -1,
            Result = true,
            Name = "DEFAULTRULE",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = DefaultDecision,
            DecisionData = string.Empty,
            OperatorConfig = string.Empty,
            StatementApplied = string.Empty,
            StatementPath = string.Empty,
            Message = string.Empty
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
            Decision = DefaultDecision,
            DecisionData = parsedRule.DecisionData,
            OperatorConfig = parsedRule.OperatorConfig,
            StatementApplied = result.StatementApplied,
            StatementPath = result.SyntaxTree,
            Message = result.Message
        };
    }

    internal static AppliedRule ErrorResult(string message)
    {
        return new AppliedRule()
        {
            Number = -1,
            Result = false,
            Name = "ERRORRULE",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = ErrorDecision,
            DecisionData = string.Empty,
            OperatorConfig = string.Empty,
            StatementApplied = string.Empty,
            StatementPath = string.Empty,
            Message = message
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
            Decision = ErrorDecision,
            DecisionData = parsedRule.DecisionData,
            OperatorConfig = parsedRule.OperatorConfig,
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
        sb.AppendLine($"Configuration: {this.OperatorConfig}");
        sb.AppendLine($"StatementApplied: {statementApplied}");
        sb.AppendLine($"StatementPath: {statementPath}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }

    private static bool IsError(RuleResult ruleResult)
    {
        return !ruleResult.Result.IsSuccess;
    }
}
