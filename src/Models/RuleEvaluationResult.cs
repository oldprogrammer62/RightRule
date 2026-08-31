using RightRule.System.Common;

namespace RightRule.Models;

public class RuleEvaluationResult
{
    public OperatorResult? OperatorResult { get; }
    public string Message { get; }
    public string StatementApplied { get; }

    private RuleEvaluationResult(OperatorResult Result, string Message, string StatementApplied)
    {
        this.OperatorResult = Result;
        this.Message = Message;
        this.StatementApplied = StatementApplied;
    }

    internal static RuleEvaluationResult Success(OperatorResult result, string successMessage, string StatementApplied)
    {
        return new RuleEvaluationResult(
            Result: result,
            Message: successMessage,
            StatementApplied: StatementApplied
        );
    }

    internal static RuleEvaluationResult Failure(OperatorResult result, string errorMessage)
    {
        return new RuleEvaluationResult(
            Result: result,
            Message: errorMessage,
            StatementApplied: string.Empty
        );
    }

    internal static RuleEvaluationResult Error(string error)
    {
        return new RuleEvaluationResult(
            Result: null,
            Message: error,
            StatementApplied: string.Empty
        );
    }
}
