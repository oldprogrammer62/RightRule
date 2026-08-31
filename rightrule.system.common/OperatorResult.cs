using System.Diagnostics.CodeAnalysis;

namespace RightRule.System.Common;

public class OperatorResult
{
    public required bool IsSuccess { get; init; }
    public required bool Result { get; init; }
    public required string Message { get; init; }
    public required object? DecisionData { get; init; }

    [SetsRequiredMembers]
    private OperatorResult(bool isSuccess, bool result, string message, object? decisionData)
    {
        IsSuccess = isSuccess;
        Result = result;
        Message = message;
        DecisionData = decisionData;
    }

    public OperatorResult Invert() => new OperatorResult(IsSuccess, !Result, Message, DecisionData);

    public static OperatorResult Success(bool result, object? decisionData) => new OperatorResult(true, result, string.Empty, decisionData);
    public static OperatorResult Failure(string message, object? decisionData) => new OperatorResult(false, false, message, decisionData);
}
