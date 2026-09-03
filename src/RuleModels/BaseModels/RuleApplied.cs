namespace RightRule.RuleModels.BaseModels;

public abstract class RuleApplied : IProcessedRule
{
    public required int Number { get; init; }
    public required string Name { get; init; }

    public required ParsedRule ParsedRule { get; init; }

    public required string StatementApplied { get; init; }

    public required bool IsSuccess { get; init; }
    public required bool Result { get; init; }


    /// <summary>
    /// The string returned to the calling application if the rule evaluates true
    /// </summary>
    public required string Decision { get; init; }

    /// <summary>
    /// Optional data returned when the Rule is evaluated to true.
    /// The DecisionData value is passed to the Operator so it can be returned.
    /// The Operator can return its own version of DecisionData if appropriate
    /// Note: the calling application should be capable of using this data when taking the appropriate action based on the decision.
    /// </summary>
    public object? DecisionData { get; init; }

    /// <summary>
    /// The string returned to the calling application if the rule evaluates true
    /// </summary>
    public required string FalseDecision { get; init; }

    /// <summary>
    /// Optional data returned when the Rule is evaluated to true.
    /// The DecisionData value is passed to the Operator so it can be returned.
    /// The Operator can return its own version of DecisionData if appropriate
    /// Note: the calling application should be capable of using this data when taking the appropriate action based on the decision.
    /// </summary>
    public object? FalseDecisionData { get; init; }
}
