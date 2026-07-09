namespace RightRule.Rules;

/// <summary>
/// Base class for all rules. A rule is a statement that can be evaluated to determine if it is true or false. It contains the context in which the rule applies, the number of the rule, the name of the rule, the statement of the rule, and the decision that should be made if the rule is true. It also contains any arguments that may be needed to evaluate the rule and any data that may be needed to make the decision.
/// </summary>
public abstract class Rule
{
    /// <summary>
    /// Provides a context for the rule.
    /// This can be used to group rules together.
    /// For example, rules with the context "Customer" would be used to evaluate rules that apply to customers with data that relates to customers
    /// </summary>
    public required string Context { get; init; }

    /// <summary>
    /// Used by RightRule to order the rules when evaluating them.
    /// Ordering is low to high.
    /// </summary>
    public required int Number { get; init; }

    /// <summary>
    /// A rule name is useful when logging
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The statement of the rule is a string that can be evaluated to determine if the rule is true or false.
    /// It must conform to the RightRule statement syntax.
    /// 
    /// </summary>
    public required string Statement { get; init; }

    /// <summary>
    /// Decision is the string returned when the rule is evaluated to true.
    /// The calling application should handle all possible decision strings and take the appropriate action based on the decision.
    /// </summary>
    public required string Decision { get; init; }

    /// <summary>
    /// Optional data returned when the rule is evaluated to true.
    /// The calling application should be capable of using this data when taking the appropriate action based on the decision.
    /// </summary>
    public object? DecisionData { get; init; }
}
