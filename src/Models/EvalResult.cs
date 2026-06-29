using System.Diagnostics.CodeAnalysis;

namespace RightRule.Models;

internal class EvalResult
{
    public required ResultType Result { get; set; }
    public required string Message { get; set; }

    [SetsRequiredMembers]
    public EvalResult()
    {
        Result = ResultType.ResultFalse;
        Message = string.Empty;
    }

    [SetsRequiredMembers]
    public EvalResult(bool ResultResult, string ResultMessage)
    {
        Result = ResultResult ? ResultType.ResultTrue : ResultType.ResultFalse;
        Message = ResultMessage;
    }

    public enum ResultType
    {
        ResultFalse = 0,
        ResultTrue = 1,
        ResultError = 9
    }
}
