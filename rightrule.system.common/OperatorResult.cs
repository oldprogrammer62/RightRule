using System.Diagnostics.CodeAnalysis;

namespace RightRule.System.Common;

public class OperatorResult
{
    public required string OperatorName { get; init; }
    public required bool Success { get; init; }
    public required bool Result { get; init; }
    public required string Message { get; init; }

    [SetsRequiredMembers]
    private OperatorResult(string operatorName, bool Success, bool result, string message)
    {
        OperatorName = operatorName;
        this.Success = Success;
        Result = result;
        Message = message;
    }

    public OperatorResult Invert() => new OperatorResult(OperatorName, Success, !Result, Message);
    public static OperatorResult SuccessResult(string operatorName, bool result) => new OperatorResult(operatorName, true, result, string.Empty);
    public static OperatorResult FailureResult(string operatorName, string message) => new OperatorResult(operatorName, false, false, message);
}
