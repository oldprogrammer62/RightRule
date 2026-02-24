using RightRule.System.Common;

namespace Operator.doesnotcontain;

public sealed class DoesNotContains : IOperator
{
    public List<string> Identifiers => ["doesnotcontain", "notc", "notni"];

    public bool Process(object left, object right)
    {
        if (left is null || right is null) return false;

        // String contains (case-insensitive)
        if (left is string ls)
            return ls.IndexOf(right.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase) < 0;

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return lstr.IndexOf(rstr, StringComparison.OrdinalIgnoreCase) < 0;
    }
}
