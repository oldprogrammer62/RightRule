using RightRule.System.Common;
using System.Diagnostics.CodeAnalysis;

namespace RightRule.Models;

internal class RuleResult
{
    public required EvaluationResult Result { get; init; }
    public required string Message { get; init; }
    public required string StatementApplied { get; init; }
    public required string SyntaxTree { get; init; }

    [SetsRequiredMembers]
    internal RuleResult(EvaluationResult Result)
    {
        this.Result = Result;
        Message = Result.Message;
        StatementApplied = string.Empty;
        SyntaxTree = string.Empty;
    }

    [SetsRequiredMembers]
    internal RuleResult(EvaluationResult Result, string ResultMessage, string StatementApplied, string SyntaxTree)
    {
        this.Result = Result;
        Message = ResultMessage;
        this.StatementApplied = StatementApplied;
        this.SyntaxTree = SyntaxTree;
    }

    internal static RuleResult Failure(string error)
    {
        return new RuleResult(
            Result: EvaluationResult.Failure(error),
            ResultMessage: error,
            StatementApplied: string.Empty,
            SyntaxTree: string.Empty
        );
    }
}
