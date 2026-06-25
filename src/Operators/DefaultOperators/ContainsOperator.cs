using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class ContainsOperator : IOperator
{
    public List<string> Identifiers => ["contains"];

    public bool Process(object left, object right)
    {
        if (left is null || right is null) return false;

        // String contains (case-insensitive)
        if (left is string ls)
            return ls.Contains(right.ToString() ?? string.Empty, StringComparison.OrdinalIgnoreCase);

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return lstr.Contains(rstr, StringComparison.OrdinalIgnoreCase);
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
}
