using RightRule.System.Common;

namespace RightRule.Models;

public class RuleEvaluationResult
{
    public EvaluationResult Result { get; }
    public string Message { get; }
    public string StatementApplied { get; }

    private RuleEvaluationResult(EvaluationResult Result, string Message, string StatementApplied)
    {
        this.Result = Result;
        this.Message = Message;
        this.StatementApplied = StatementApplied;
    }

    internal static RuleEvaluationResult Success(EvaluationResult result, string successMessage, string StatementApplied)
    {
        return new RuleEvaluationResult(
            Result: result,
            Message: successMessage,
            StatementApplied: StatementApplied
        );
    }

    internal static RuleEvaluationResult Failure(EvaluationResult result, string errorMessage)
    {
        return new RuleEvaluationResult(
            Result: result,
            Message: errorMessage,
            StatementApplied: string.Empty
        );
    }

    internal static RuleEvaluationResult Failure(string error)
    {
        return new RuleEvaluationResult(
            Result: EvaluationResult.Failure(error),
            Message: error,
            StatementApplied: string.Empty
        );
    }
}
