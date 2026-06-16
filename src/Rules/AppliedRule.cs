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
    /// Provides a list of all the rules that were evaluated and matched when evaluating the rule. This is useful for logging and debugging purposes, as it allows the calling application to know which rules were evaluated and matched when making the decision. It can also be used to provide additional context when taking action based on the decision, such as including the names of the matched rules in a notification or report.
    /// </summary>
    public required List<ParsedRule> RulesMatched { get; init; }

    /// <summary>
    /// Provides a list of all the statements that were applied when evaluating the rule. This is useful for logging and debugging purposes, as it allows the calling application to know which statements were applied when making the decision. It can also be used to provide additional context when taking action based on the decision, such as including the statements in a notification or report.
    /// </summary>
    public required List<string> StatementApplied { get; init; }

    internal static readonly string NoDecision = "NODECISION";
    internal static readonly string NoDatasetMatched = "NO_DATASET_MATCHED";

    public AppliedRule() {}

    [SetsRequiredMembers]
    internal AppliedRule(ParsedRule parsedRule, string datasetMatched, string statementApplied, bool result)
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
        RulesMatched = [parsedRule];
        StatementApplied = [statementApplied];
    }

    [SetsRequiredMembers]
    internal AppliedRule(ParsedRule parsedRule, string datasetMatched, List<ParsedRule> rulesMatched, List<string> statementsApplied, bool result)
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
        RulesMatched = rulesMatched;
        StatementApplied = statementsApplied;
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
            RulesMatched = new List<ParsedRule>(),
            StatementApplied = new List<string>()
        };
    }
}
