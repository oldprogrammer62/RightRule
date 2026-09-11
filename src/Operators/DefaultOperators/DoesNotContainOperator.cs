using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class DoesNotContainOperator : Operator
{
    public override List<string> Identifiers { get; } = ["doesnotcontain", "notc"];

    public DoesNotContainOperator() : base(false)
    {
    }

    protected override async Task<OperatorResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, bool Diagnostics)
    {
        if (left is null || right is null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }

        // String contains (case-insensitive)
        if (left is string ls && right is string rs)
        {
            return OperatorResult.SuccessResult(this.Name, NotContains(ls, rs));
        }

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return OperatorResult.SuccessResult(this.Name, NotContains(lstr, rstr));
    }

    private static bool NotContains(string left, string right)
    {
        return !(left.Contains(right, StringComparison.OrdinalIgnoreCase));
    }
}
