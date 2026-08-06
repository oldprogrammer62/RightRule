using RightRule.System.Common;

namespace RightRule.Models;

internal class RuleResult
{
    public EvaluationResult Result { get; }
    public string Message { get; }
    public string StatementApplied { get; }
    public string SyntaxTree { get; }

    private RuleResult(EvaluationResult Result, string Message, string StatementApplied, string SyntaxTree)
    {
        this.Result = Result;
        this.Message = Message;
        this.StatementApplied = StatementApplied;
        this.SyntaxTree = SyntaxTree;
    }

    internal static RuleResult Success(EvaluationResult result, string successMessage, string StatementApplied, string SyntaxTree)
    {
        return new RuleResult(
            Result: result,
            Message: successMessage,
            StatementApplied: StatementApplied,
            SyntaxTree: SyntaxTree
        );
    }

    internal static RuleResult Failure(EvaluationResult result, string errorMessage)
    {
        return new RuleResult(
            Result: result,
            Message: errorMessage,
            StatementApplied: string.Empty,
            SyntaxTree: string.Empty
        );
    }

    internal static RuleResult Failure(string error)
    {
        return new RuleResult(
            Result: EvaluationResult.Failure(error),
            Message: error,
            StatementApplied: string.Empty,
            SyntaxTree: string.Empty
        );
    }
}
