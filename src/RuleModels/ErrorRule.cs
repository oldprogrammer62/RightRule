using RightRule.Models;
using RightRule.RuleModels.BaseModels;
using System.Text;

namespace RightRule.RuleModels;

/// <summary>
/// Represents a rule that has been applied and evaluated as true against a dataset.
/// </summary>
public sealed class ErrorRule : RuleError
{
    private ErrorRule() : base()
    {
    }

    internal static ErrorRule ErrorResult(string message)
    {
        return new ErrorRule() 
        {
            Message = message
        };
    }

    internal static ErrorRule ErrorResult(ParsedRule parsedRule, RuleEvaluationResult result)
    {
        return new ErrorRule()
        {
            Message = result.Message
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Decision: {RuleError.DECISIONWORD}");
        sb.AppendLine($"Message: {this.Message}");
        return sb.ToString();
    }
}
