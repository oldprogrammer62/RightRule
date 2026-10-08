using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class DoesNotContainOperator : Operator
{
    public override List<string> Aliases { get; } = ["doesnotcontain", "notc"];

    public DoesNotContainOperator() : base(false)
    {
    }

    protected override async Task<OperatorResult> ExecuteOperatorProcessAsync(object Left, object Right, object? OptionalData, bool Diagnostics)
    {
        if (Left is null || Right is null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }

        // String contains (case-insensitive)
        if (Left is string ls && Right is string rs)
        {
            return OperatorResult.SuccessResult(this.Name, NotContains(ls, rs));
        }

        // Fallback to string contains
        var lstr = Left.ToString() ?? string.Empty;
        var rstr = Right.ToString() ?? string.Empty;
        return OperatorResult.SuccessResult(this.Name, NotContains(lstr, rstr));
    }

    private static bool NotContains(string Left, string Right)
    {
        return !(Left.Contains(Right, StringComparison.OrdinalIgnoreCase));
    }
}
