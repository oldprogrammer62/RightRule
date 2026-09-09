using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration;
using System.Text;

namespace RightRule.Models.RuleModels;

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

    internal static ErrorRule ErrorResult(ParsedRule parsedRule, EvaluationResult result)
    {
        return new ErrorRule()
        {
            Message = result.Message
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {(int)RuleNumberEnum.ERRORRULE}");
        sb.AppendLine($"Decision: {DECISIONWORD}");
        sb.AppendLine($"Message: {Message}");
        return sb.ToString();
    }
}
