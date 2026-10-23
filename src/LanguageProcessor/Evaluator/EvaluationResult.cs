using System.Diagnostics.CodeAnalysis;

namespace RightRule.LanguageProcessor.Evaluator;

public sealed class EvaluationResult
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

    internal static EvaluationResult SuccessResult(bool Success, bool Result, string Message, string StatementApplied)
    {
        return new EvaluationResult(
            Success: Success,
            Result: Result,
            Message: Message,
            StatementApplied: StatementApplied
        );
    }

    internal static EvaluationResult FailureResult(bool Success, bool Result, string Message, string StatementApplied)
    {
        return new EvaluationResult(
            Success: Success,
            Result: Result,
            Message: Message,
            StatementApplied: StatementApplied
        );
    }

    internal static EvaluationResult ErrorResult(string Message)
    {
        return new EvaluationResult(
            Success: false,
            Result: false,
            Message: Message,
            StatementApplied: string.Empty
        );
    }
}
