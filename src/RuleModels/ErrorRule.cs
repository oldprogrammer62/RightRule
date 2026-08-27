using RightRule.Models;
using System.Text;

namespace RightRule.RuleModels;

/// <summary>
/// Represents a rule that has been applied and evaluated as true against a dataset.
/// </summary>
public sealed class ErrorRule : Rule
{
    /// <summary>
    /// The logic of the Rule, evaluated to determine if the rule is true or false.
    /// The statement must conform to the RightRule statement syntax.
    /// </summary>
    public required string Statement { get; init; }

    /// <summary>
    /// Used to return a message
    /// - Exception message if an error is thrown
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Decision returned when an error occurs during Rule evaluation.
    /// </summary>
    public static string ErrorDecision { get; } = "ERROR";

    private ErrorRule() : base()
    {
        Decision = ErrorDecision;
    }

    internal static ErrorRule ErrorResult(string message)
    {
        return new ErrorRule()
        {
            Number = -1,
            Name = "ERRORRULE",
            Statement = string.Empty,
            Context = string.Empty,
            Decision = ErrorDecision,
            DecisionData = string.Empty,
            Message = message
        };
    }

    internal static ErrorRule ErrorResult(ParsedRule parsedRule, RuleEvaluationResult result)
    {
        return new ErrorRule()
        {
            Number = parsedRule.Number,
            Name = parsedRule.Name,
            Statement = parsedRule.Statement,
            Context = parsedRule.Context,
            Decision = ErrorDecision,
            DecisionData = result.EvaluationResult.DecisionData,
            Message = result.Message
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.Number}");
        sb.AppendLine($"Name: {this.Name}");
        sb.AppendLine($"Context: {this.Context}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }
}
