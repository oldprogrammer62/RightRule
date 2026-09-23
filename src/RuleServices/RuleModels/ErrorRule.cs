using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleServices.RuleModels.BaseModels;
using System.Text;

namespace RightRule.RuleServices.RuleModels;

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
            Context = parsedRule.Context,
            StatementApplied = result.StatementApplied,
            Message = result.Message
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {Number}");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Context: {Context}");
        sb.AppendLine($"Decision: {Decision}");
        sb.AppendLine($"DecisionData: {DecisionData}");
        sb.AppendLine($"StatementApplied: {StatementApplied}");
        sb.AppendLine($"Message: {Message}");
        return sb.ToString();
    }
}
