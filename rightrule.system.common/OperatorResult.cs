using System.Diagnostics.CodeAnalysis;

namespace RightRule.System.Common;

public class OperatorResult
{
    public required string OperatorName { get; init; }
    public required bool Success { get; init; }
    public required bool Result { get; init; }
    public required string Message { get; init; }

    [SetsRequiredMembers]
    private OperatorResult(string OperatorName, bool Success, bool Result, string Message)
    {
        this.OperatorName = OperatorName;
        this.Success = Success;
        this.Result = Result;
        this.Message = Message;
    }

    public OperatorResult Invert() => new OperatorResult(OperatorName, Success, !Result, Message);
    public static OperatorResult SuccessResult(string OperatorName, bool Result) => new OperatorResult(OperatorName, true, Result, string.Empty);
    public static OperatorResult FailureResult(string OperatorName, string Message) => new OperatorResult(OperatorName, false, false, Message);
}
