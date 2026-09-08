using System.Diagnostics.CodeAnalysis;

namespace RightRule.RuleModels.RuleEvaluation;

public class RuleEvaluationResult
{
    public required bool IsSuccess { get; init; }
    public required bool Result { get; init; }

    public string Message { get; }
    public string StatementApplied { get; }

    [SetsRequiredMembers]
    private RuleEvaluationResult(bool Success, bool Result, string Message, string StatementApplied)
    {
        IsSuccess = Success;
        this.Result = Result;
        this.Message = Message;
        this.StatementApplied = StatementApplied;
    }

    internal static RuleEvaluationResult Success(bool success, bool result, string successMessage, string StatementApplied)
    {
        return new RuleEvaluationResult(
            Success: success,
            Result: result,
            Message: successMessage,
            StatementApplied: StatementApplied
        );
    }

    internal static RuleEvaluationResult Failure(bool success, bool result, string errorMessage)
    {
        return new RuleEvaluationResult(
            Success: success,
            Result: result,
            Message: errorMessage,
            StatementApplied: string.Empty
        );
    }

    internal static RuleEvaluationResult Error(string error)
    {
        return new RuleEvaluationResult(
            Success: false,
            Result: false,
            Message: error,
            StatementApplied: string.Empty
        );
    }
}
