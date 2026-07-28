using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class DoesNotContainOperator : Operator
{
    public override List<string> Identifiers { get; } = ["doesnotcontain", "notc", "notni"];

    protected override async Task<bool> DoOperatorProcessAsync(object left, object right)
    {
        return await Task.Run(() =>
        {
            if (left is null || right is null) return false;

            // String contains (case-insensitive)
            if (left is string ls && right is string rs)
                return NotContains(ls, rs);

            // Fallback to string contains
            var lstr = left.ToString() ?? string.Empty;
            var rstr = right.ToString() ?? string.Empty;
            return NotContains(lstr, rstr);
        });
    }

    private static bool NotContains(string left, string right)
    {
        return !(left.Contains(right, StringComparison.OrdinalIgnoreCase));
    }
}
