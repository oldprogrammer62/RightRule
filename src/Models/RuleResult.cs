using RightRule.System.Common;

namespace RightRule.Models;

public class RuleResult
{
    public EvaluationResult EvaluationResult { get; }
    public string Message { get; }
    public string StatementApplied { get; }

    private RuleResult(EvaluationResult Result, string Message, string StatementApplied)
    {
        this.EvaluationResult = Result;
        this.Message = Message;
        this.StatementApplied = StatementApplied;
    }

    internal static RuleResult Success(EvaluationResult result, string successMessage, string StatementApplied)
    {
        return new RuleResult(
            Result: result,
            Message: successMessage,
            StatementApplied: StatementApplied
        );
    }

    internal static RuleResult Failure(EvaluationResult result, string errorMessage)
    {
        return new RuleResult(
            Result: result,
            Message: errorMessage,
            StatementApplied: string.Empty
        );
    }

    internal static RuleResult Failure(string error, object? decisionData)
    {
        return new RuleResult(
            Result: EvaluationResult.Failure(error, decisionData),
            Message: error,
            StatementApplied: string.Empty
        );
    }
}
