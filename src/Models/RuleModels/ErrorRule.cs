using RightRule.RegistrationServices.Rules;
using RightRule.RuleModels.BaseModels;
using RightRule.RuleModels.RuleEvaluation;
using System.Text;

namespace RightRule.RuleModels.RuleModels;

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
        sb.AppendLine($"Number: {(int)RuleNumberEnum.ErrorRule}");
        sb.AppendLine($"Decision: {DECISIONWORD}");
        sb.AppendLine($"Message: {Message}");
        return sb.ToString();
    }
}
