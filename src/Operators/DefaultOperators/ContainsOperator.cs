using RightRule.System.Common.Operator.Models;

namespace RightRule.Operators.DefaultOperators;

public sealed class ContainsOperator : Operator
{
    public ContainsOperator() : base(false)
    {
    }

    public override List<string> Aliases { get;  } = ["contains"];

    protected override async Task<OperatorResult> ExecuteOperatorProcessAsync(object Left, object Right, object? OptionalData, bool Diagnostics)
    {
        if (Left is null || Right is null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }

        // String contains (case-insensitive)
        if (Left is string ls && Right is string rs)
        {
            return OperatorResult.SuccessResult(this.Name, Contains(ls, rs));
        }

        // Fallback to string contains
        var lstr = Left.ToString() ?? string.Empty;
        var rstr = Right.ToString() ?? string.Empty;
        return OperatorResult.SuccessResult(this.Name, Contains(lstr, rstr));
    }

    private static bool Contains(string Left, string Right)
    {
        return Left.Contains(Right, StringComparison.OrdinalIgnoreCase);
    }
}
