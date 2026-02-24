using System.Collections;
using RightRule.System.Common;

namespace RightRule.Operators;

public sealed class ContainsOperator : IOperator
{
    public List<string> Names => ["contains"];

    public bool Process(object left, object right)
    {
        if (left is null || right is null) return false;

        // String contains (case-insensitive)
        if (left is string ls)
            return ls.IndexOf(right.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0;

        // IEnumerable contains (non-string)
        if (left is IEnumerable seq && left is not string)
        {
            foreach (var item in seq)
            {
                if (Equals(item, right)) return true;
            }
            return false;
        }

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return lstr.IndexOf(rstr, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
