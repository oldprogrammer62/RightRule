using RightRule.System.Common;

namespace Operator.doesnotcontain;

public sealed class DoesNotContains : IOperator
{
    public List<string> Names => ["doesnotcontain", "notc"];

    public bool Process(object left, object right)
    {
        if (left is null || right is null) return false;

        // String contains (case-insensitive)
        if (left is string ls)
            return ls.IndexOf(right.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase) < 0;

        // IEnumerable contains (non-string)
        if (left is System.Collections.IEnumerable seq && left is not string)
        {
            foreach (var item in seq)
            {
                if (!Equals(item, right)) return true;
            }
            return false;
        }

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return lstr.IndexOf(rstr, StringComparison.OrdinalIgnoreCase) < 0;
    }
}
