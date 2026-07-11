using System.Diagnostics.CodeAnalysis;

namespace RightRule.Models;

internal class EvalResult
{
    public required ResultType Result { get; set; }
    public required string Message { get; set; }
    public required string StatementApplied { get; set; }

    [SetsRequiredMembers]
    public EvalResult()
    {
        Result = ResultType.ResultFalse;
        Message = string.Empty;
        StatementApplied = string.Empty;
    }

    [SetsRequiredMembers]
    public EvalResult(bool ResultResult, string ResultMessage, string StatementApplied)
    {
        Result = ResultResult ? ResultType.ResultTrue : ResultType.ResultFalse;
        Message = ResultMessage;
        this.StatementApplied = StatementApplied;
    }

    public enum ResultType
    {
        ResultFalse = 0,
        ResultTrue = 1,
        ResultError = 9
    }
}
