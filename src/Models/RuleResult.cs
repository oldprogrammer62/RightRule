using RightRule.System.Common;

namespace RightRule.Models;

internal class RuleResult
{
    public EvaluationResult Result { get; }
    public string Message { get; }
    public string StatementApplied { get; }

    private RuleResult(EvaluationResult Result, string Message, string StatementApplied)
    {
        this.Result = Result;
        this.Message = Message;
        this.StatementApplied = StatementApplied;
    }

    internal static RuleResult Success(EvaluationResult result, string successMessage, string StatementApplied)
    {
        return new RuleResult(
            Result: result,
            Message: successMessage,
            StatementApplied: StatementApplied
        );
    }

    internal static RuleResult Failure(EvaluationResult result, string errorMessage)
    {
        return new RuleResult(
            Result: result,
            Message: errorMessage,
            StatementApplied: string.Empty
        );
    }

    internal static RuleResult Failure(string error)
    {
        return new RuleResult(
            Result: EvaluationResult.Failure(error),
            Message: error,
            StatementApplied: string.Empty
        );
    }
}
