using System.Diagnostics.CodeAnalysis;

namespace RightRule.Models;

internal class EvalResult
{
    public required ResultType Result { get; init; }
    public required string Message { get; init; }
    public required string StatementApplied { get; init; }
    public required string SyntaxTree { get; init; }

    [SetsRequiredMembers]
    public EvalResult()
    {
        Result = ResultType.ResultFalse;
        Message = string.Empty;
        StatementApplied = string.Empty;
        SyntaxTree = string.Empty;
    }

    [SetsRequiredMembers]
    public EvalResult(bool ResultResult, string ResultMessage, string StatementApplied, string SyntaxTree)
    {
        Result = ResultResult ? ResultType.ResultTrue : ResultType.ResultFalse;
        Message = ResultMessage;
        this.StatementApplied = StatementApplied;
        this.SyntaxTree = SyntaxTree;
    }

    public enum ResultType
    {
        ResultFalse = 0,
        ResultTrue = 1,
        ResultError = 9
    }
}
