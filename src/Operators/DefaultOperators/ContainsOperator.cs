using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class ContainsOperator : Operator
{
    public ContainsOperator() : base(false)
    {
    }

    public override List<string> Identifiers { get;  } = ["contains"];

    protected override async Task<OperatorResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, bool Diagnostics)
    {
        if (left is null || right is null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }

        // String contains (case-insensitive)
        if (left is string ls && right is string rs)
        {
            return OperatorResult.SuccessResult(this.Name, Contains(ls, rs));
        }

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return OperatorResult.SuccessResult(this.Name, Contains(lstr, rstr));
    }

    private static bool Contains(string left, string right)
    {
        return left.Contains(right, StringComparison.OrdinalIgnoreCase);
    }
}
