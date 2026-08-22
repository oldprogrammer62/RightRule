using RightRule.Models;
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
    /// </summary>
    public string StatementApplied { get; init; }

    /// <summary>
    /// Decision returned when no Rule evaluates to true for the specified Context.
    /// </summary>
    public static string DefaultDecision { get; } = "NODECISION";

    /// <summary>
    /// Decision returned when an error occurs during Rule evaluation.
    /// </summary>
    public static string ErrorDecision { get; } = "ERROR";

    private AppliedRule() : base()
    {
        StatementApplied = string.Empty;
    }

    internal static AppliedRule TrueResult(ParsedRule parsedRule, RuleEvaluationResult result)
    {
        return new AppliedRule()
        {
            Number = parsedRule.Number,
            Result = true,
            Name = parsedRule.Name,
            Statement = parsedRule.Statement,
            Context = parsedRule.Context,
            Decision = parsedRule.Decision,
            DecisionData = parsedRule.DecisionData,
            OptionalData = parsedRule.OptionalData,
            StatementApplied = result.StatementApplied,
            RuleConfiguration = parsedRule.RuleConfiguration,
            Message = result.Message
        };
    }

    internal static AppliedRule FalseResult(ParsedRule parsedRule, RuleEvaluationResult result)
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
            OptionalData = parsedRule.OptionalData,
            StatementApplied = result.StatementApplied,
            RuleConfiguration = parsedRule.RuleConfiguration,
            Message = result.Message
        };
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
            OptionalData = string.Empty,
            StatementApplied = string.Empty,
            RuleConfiguration = RuleConfiguration.DefaultConfig(),
            Message = string.Empty
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
            OptionalData = string.Empty,
            StatementApplied = string.Empty,
            RuleConfiguration = null,
            Message = message
        };
    }

    internal static AppliedRule ErrorResult(ParsedRule parsedRule, RuleEvaluationResult result)
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
            OptionalData = parsedRule.OptionalData,
            StatementApplied = result.StatementApplied,
            RuleConfiguration = parsedRule.RuleConfiguration,
            Message = result.Message
        };
    }

    public override string ToString()
    {
        var statementApplied = !string.IsNullOrWhiteSpace(StatementApplied) ? StatementApplied : "Enable Diagnostics";

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.Number}");
        sb.AppendLine($"Name: {this.Name}");
        sb.AppendLine($"Context: {this.Context}");
        sb.AppendLine($"Statement: {this.Statement}");
        sb.AppendLine($"Result: {this.Result}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        sb.AppendLine($"Configuration: {this.OptionalData}");
        sb.AppendLine($"StatementApplied: {statementApplied}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }
}
