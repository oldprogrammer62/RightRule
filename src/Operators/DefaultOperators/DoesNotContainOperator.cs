using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class DoesNotContainOperator : Operator
{
    public override List<string> Identifiers { get; } = ["doesnotcontain", "notc", "notni"];

    public DoesNotContainOperator() : base(false)
    {
    }

    protected override async Task<EvaluationResult> DoOperatorProcessAsync(object left, object right, object? OptionalData)
    {
        if (left is null || right is null)
        {
            return EvaluationResult.Failure("One or both operands are null.");
        }

        // String contains (case-insensitive)
        if (left is string ls && right is string rs)
        {
            return EvaluationResult.Success(NotContains(ls, rs));
        }

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return EvaluationResult.Success(NotContains(lstr, rstr));
    }

    private static bool NotContains(string left, string right)
    {
        return !(left.Contains(right, StringComparison.OrdinalIgnoreCase));
    }
}
