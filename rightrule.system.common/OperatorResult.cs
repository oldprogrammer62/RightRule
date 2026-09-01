using System.Diagnostics.CodeAnalysis;

namespace RightRule.System.Common;

public class OperatorResult
{
    public required bool IsSuccess { get; init; }
    public required bool Result { get; init; }
    public required string Message { get; init; }
    public required object? DecisionData { get; init; }
    public required object? FalseDecisionData { get; init; }

    [SetsRequiredMembers]
    private OperatorResult(bool isSuccess, bool result, string message, object? decisionData, object? falseDecisionData)
    {
        IsSuccess = isSuccess;
        Result = result;
        Message = message;
        DecisionData = decisionData;
        FalseDecisionData = falseDecisionData;
    }

    public OperatorResult Invert() => new OperatorResult(IsSuccess, !Result, Message, DecisionData, FalseDecisionData);

    public static OperatorResult Success(bool result, object? decisionData, object? falseDecisionData) => new OperatorResult(true, result, string.Empty, decisionData, falseDecisionData);
    public static OperatorResult Failure(string message, object? decisionData, object? falseDecisionData) => new OperatorResult(false, false, message, decisionData, falseDecisionData);
}
