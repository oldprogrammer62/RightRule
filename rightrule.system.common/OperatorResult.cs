using System.Diagnostics.CodeAnalysis;

namespace RightRule.System.Common;

public class OperatorResult
{
    public required string OperatorName { get; init; }
    public required bool IsSuccess { get; init; }
    public required bool Result { get; init; }
    public required string Message { get; init; }

    [SetsRequiredMembers]
    private OperatorResult(string operatorName, bool isSuccess, bool result, string message)
    {
        OperatorName = operatorName;
        IsSuccess = isSuccess;
        Result = result;
        Message = message;
    }

    public OperatorResult Invert() => new OperatorResult(OperatorName, IsSuccess, !Result, Message);

    public static OperatorResult Success(string operatorName, bool result) => new OperatorResult(operatorName, true, result, string.Empty);
    public static OperatorResult Failure(string operatorName, string message) => new OperatorResult(operatorName, false, false, message);
}
