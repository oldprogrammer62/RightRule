namespace RightRule.RuleModels.BaseModels;

/// <summary>
/// Base class for all rules. A rule is a statement that can be evaluated to determine if it is true or false. It contains the context in which the rule applies, the number of the rule, the name of the rule, the statement of the rule, and the decision that should be made if the rule is true. It also contains any arguments that may be needed to evaluate the rule and any data that may be needed to make the decision.
/// </summary>
public abstract class Rule : IRule
{
    /// <summary>
    /// Identifies the Rule by number.
    /// Used when ordering rules for evaluation. Ordering is low to high.
    /// </summary>
    public required int Number { get; init; }

    /// <summary>
    /// Identifies the Rule by name.
    /// </summary>
    public required string Name { get; init; }

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




}
