using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class ContainsOperator : Operator
{
    public ContainsOperator() : base(false)
    {
    }

    public override List<string> Identifiers { get;  } = ["contains"];

    protected override async Task<bool> DoOperatorProcessAsync(object left, object right)
    {
        if (left is null || right is null) return false;

        // String contains (case-insensitive)
        if (left is string ls && right is string rs)
            return Contains(ls, rs);

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return Contains(lstr, rstr);
    }

    private static bool Contains(string left, string right)
    {
        return left.Contains(right, StringComparison.OrdinalIgnoreCase);
    }
}
