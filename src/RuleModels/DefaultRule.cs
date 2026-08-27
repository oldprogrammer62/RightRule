using System.Text;

namespace RightRule.RuleModels;

/// <summary>
/// Represents a rule that has been applied and evaluated as true against a dataset.
/// </summary>
public sealed class DefaultRule : Rule
{
    /// <summary>
    /// The logic of the Rule, evaluated to determine if the rule is true or false.
    /// The statement must conform to the RightRule statement syntax.
    /// </summary>
    public required string Statement { get; init; }

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
    /// Decision returned when no Rule evaluates to true for the specified Context.
    /// </summary>
    public static string DefaultDecision { get; } = "NODECISION";

    private DefaultRule() : base()
    {
        Decision = DefaultDecision;
    }

    internal static DefaultRule DefaultResult()
    {
        return new DefaultRule()
        {
            Number = -1,
            Result = true,
            Name = "DEFAULTRULE",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = DefaultDecision,
            DecisionData = string.Empty,
            Message = string.Empty
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.Number}");
        sb.AppendLine($"Name: {this.Name}");
        sb.AppendLine($"Context: {this.Context}");
        sb.AppendLine($"Statement: {this.Statement}");
        sb.AppendLine($"Result: {this.Result}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }
}
