namespace RightRule.RuleModels.BaseModels;

public abstract class RuleApplied : IRule
{
    public required ParsedRule ParsedRule { get; init; }

    //public required RuleDefinition RuleDefinition { get; init; }

    /// <summary>
    /// Diagnostics only
    /// Provides the actual statement applied when evaluating the rule.
    /// </summary>
    public required string StatementApplied { get; init; }

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
