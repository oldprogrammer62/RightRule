using RightRule.Models;
using System.Diagnostics.CodeAnalysis;

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
    /// The name of the dataset that was matched when evaluating the rule.
    /// This is useful for logging and debugging purposes, as it allows the calling application to know which dataset was used to evaluate the rule and make the decision. It can also be used to provide additional context when taking action based on the decision, such as including the dataset name in a notification or report.
    /// </summary>
    public required string DatasetMatched { get; init; }

    /// <summary>
    /// Provides the actual statement applied when evaluating the rule. This is useful for logging and debugging purposes, as it allows the calling application to know which statements were applied when making the decision. It can also be used to provide additional context when taking action based on the decision, such as including the statements in a notification or report.
    /// All placeholder arguments will be replaced with their actual values. This allows the calling application to see the exact statement that was evaluated against the dataset, which can be helpful for understanding the decision-making process and for troubleshooting any issues that may arise.
    /// </summary>
    public required string StatementApplied { get; init; }

    /// <summary>
    /// Provides the error message, if the application of a rule throws an exception
    /// </summary>
    public required string ErrorMessage { get; init; }

    internal static readonly string NoDecision = "NODECISION";
    internal static readonly string NoDatasetMatched = "NO_DATASET_MATCHED";

    public AppliedRule() {}

    private bool IsError(EvalResult ruleResult)
    {
        return ruleResult.Result.Equals(EvalResult.ResultType.ResultError);
    }

    [SetsRequiredMembers]
    internal AppliedRule(ParsedRule parsedRule, string datasetMatched, string statementApplied, EvalResult result)
    {
        if (IsError(result))
        {
            Result = false;
            Arguments = null;
            Number = -1;
            Name = "DEFAULTRULE";
            Statement = string.Empty;
            Context = string.Empty;
            Decision = NoDecision;
            DecisionData = string.Empty;
            DatasetMatched = NoDatasetMatched;
            StatementApplied = string.Empty;
            ErrorMessage = result.Message;
            return;
        }

        Result = result.Result.Equals(EvalResult.ResultType.ResultTrue);
        Arguments = parsedRule.Arguments;
        Number = parsedRule.Number;
        Name = parsedRule.Name;
        Statement = parsedRule.Statement;
        Context = parsedRule.Context;
        Decision = parsedRule.Decision;
        DecisionData = parsedRule.DecisionData;
        DatasetMatched = datasetMatched;
        StatementApplied = statementApplied;
        ErrorMessage = result.Message;
    }

    [SetsRequiredMembers]
    internal AppliedRule(ParsedRule parsedRule, string datasetMatched, List<ParsedRule> rulesMatched, string statementApplied, bool result, string message)
    {
        Result = result;
        Arguments = parsedRule.Arguments;
        Number = parsedRule.Number;
        Name = parsedRule.Name;
        Statement = parsedRule.Statement;
        Context = parsedRule.Context;
        Decision = parsedRule.Decision;
        DecisionData = parsedRule.DecisionData;
        DatasetMatched = datasetMatched;
        StatementApplied = statementApplied;
        ErrorMessage = message;
    }

    public static AppliedRule DefaultRule()
    {
        return DefaultRule(string.Empty);
    }

    public static AppliedRule DefaultRule(string decisionData)
    {
        return new AppliedRule()
        {
            Result = true,
            Arguments = null,
            Number = -1,
            Name = "DEFAULTRULE",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = NoDecision,
            DecisionData = decisionData,
            DatasetMatched = NoDatasetMatched,
            StatementApplied = string.Empty,
            ErrorMessage = string.Empty
        };
    }
}
