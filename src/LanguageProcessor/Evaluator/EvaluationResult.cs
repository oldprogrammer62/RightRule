using System.Diagnostics.CodeAnalysis;

namespace RightRule.LanguageProcessor.Evaluator;

public class EvaluationResult
{
    public required bool Success { get; init; }
    public required bool Result { get; init; }

    public string Message { get; }
    public string StatementApplied { get; }

    [SetsRequiredMembers]
    private EvaluationResult(bool Success, bool Result, string Message, string StatementApplied)
    {
        this.Success = Success;
        this.Result = Result;
        this.Message = Message;
        this.StatementApplied = StatementApplied;
    }

    internal static EvaluationResult SuccessResult(bool success, bool result, string successMessage, string StatementApplied)
    {
        return new EvaluationResult(
            Success: success,
            Result: result,
            Message: successMessage,
            StatementApplied: StatementApplied
        );
    }

    internal static EvaluationResult FailureResult(bool success, bool result, string errorMessage)
    {
        return new EvaluationResult(
            Success: success,
            Result: result,
            Message: errorMessage,
            StatementApplied: string.Empty
        );
    }

    internal static EvaluationResult ErrorResult(string error)
    {
        return new EvaluationResult(
            Success: false,
            Result: false,
            Message: error,
            StatementApplied: string.Empty
        );
    }
}
