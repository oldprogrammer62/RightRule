using System.Diagnostics.CodeAnalysis;

namespace RightRule.System.Common;

public class EvaluationResult
{
    public required bool IsSuccess { get; init; }
    public required bool Result { get; init; }
    public required string Message { get; init; }
    public required object? DecisionData { get; init; }

    [SetsRequiredMembers]
    private EvaluationResult(bool isSuccess, bool result, string message, object? decisionData)
    {
        IsSuccess = isSuccess;
        Result = result;
        Message = message;
        DecisionData = decisionData;
    }

    public EvaluationResult Invert() => new EvaluationResult(IsSuccess, !Result, Message, DecisionData);

    public static EvaluationResult Success(bool result, object? decisionData) => new EvaluationResult(true, result, string.Empty, decisionData);
    public static EvaluationResult Failure(string message, object? decisionData) => new EvaluationResult(false, false, message, decisionData);
}
