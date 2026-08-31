using RightRule.Models;

namespace RightRule.RuleModels.BaseModels;

public abstract class RuleDefinition : Rule
{
    /// <summary>
    /// Binds the Rule to a specific event context defined in code.
    /// </summary>
    public required string Context { get; init; }

    /// <summary>
    /// The logic of the Rule, evaluated to determine if the rule is true or false.
    /// The statement must conform to the RightRule statement syntax.
    /// </summary>
    public required string Statement { get; init; }

    /// <summary>
    /// The string returned to the calling application if the rule evaluates true
    /// </summary>
    //public string Decision { get; } = "";

    /// <summary>
    /// Optional data returned when the Rule is evaluated to true.
    /// The DecisionData value is passed to the Operator so it can be returned.
    /// The Operator can return its own version of DecisionData if appropriate
    /// Note: the calling application should be capable of using this data when taking the appropriate action based on the decision.
    /// </summary>
    //public object? DecisionData { get; init; }

    // TO DO False details
    /// <summary>
    /// The string returned to the calling application if the rule evaluates true
    /// </summary>
    //public required string FalseDecision { get; init; }

    /// <summary>
    /// Optional data returned when the Rule is evaluated to true.
    /// The DecisionData value is passed to the Operator so it can be returned.
    /// The Operator can return its own version of DecisionData if appropriate
    /// Note: the calling application should be capable of using this data when taking the appropriate action based on the decision.
    /// </summary>
    //public object? FalseDecisionData { get; init; }

    /// <summary>
    /// Optional data that will be passed to the Operator when evaluating the Rule.
    /// It is the responsibility of the Operator to validate the configuration details and use it as required.
    /// </summary>
    public object? OptionalData { get; init; }

    /// <summary>
    /// Configuration details
    /// </summary>
    public RightRuleConfiguration RuleConfiguration { get; init; } = new RightRuleConfiguration();
}
