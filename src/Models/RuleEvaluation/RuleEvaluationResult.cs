using System.Diagnostics.CodeAnalysis;

namespace RightRule.Models.RuleEvaluation;

public class RuleEvaluationResult
{
    public required bool Success { get; init; }
    public required bool Result { get; init; }

    public string Message { get; }
    public string StatementApplied { get; }

    [SetsRequiredMembers]
    private RuleEvaluationResult(bool Success, bool Result, string Message, string StatementApplied)
    {
        this.Success = Success;
        this.Result = Result;
        this.Message = Message;
        this.StatementApplied = StatementApplied;
    }

    internal static RuleEvaluationResult SuccessResult(bool success, bool result, string successMessage, string StatementApplied)
    {
        return new RuleEvaluationResult(
            Success: success,
            Result: result,
            Message: successMessage,
            StatementApplied: StatementApplied
        );
    }

    internal static RuleEvaluationResult FailureResult(bool success, bool result, string errorMessage)
    {
        return new RuleEvaluationResult(
            Success: success,
            Result: result,
            Message: errorMessage,
            StatementApplied: string.Empty
        );
    }

    internal static RuleEvaluationResult ErrorResult(string error)
    {
        return new RuleEvaluationResult(
            Success: false,
            Result: false,
            Message: error,
            StatementApplied: string.Empty
        );
    }
}
