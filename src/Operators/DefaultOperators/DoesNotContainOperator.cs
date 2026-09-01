using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class DoesNotContainOperator : Operator
{
    public override List<string> Identifiers { get; } = ["doesnotcontain", "notc", "notni"];

    public DoesNotContainOperator() : base(false)
    {
    }

    protected override async Task<OperatorResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, object? DecisionData, object? FalseDecisionData, bool Diagnostics)
    {
        if (left is null || right is null)
        {
            return OperatorResult.Failure("One or both operands are null.", DecisionData, FalseDecisionData);
        }

        // String contains (case-insensitive)
        if (left is string ls && right is string rs)
        {
            return OperatorResult.Success(NotContains(ls, rs), DecisionData, FalseDecisionData);
        }

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return OperatorResult.Success(NotContains(lstr, rstr), DecisionData, FalseDecisionData);
    }

    private static bool NotContains(string left, string right)
    {
        return !(left.Contains(right, StringComparison.OrdinalIgnoreCase));
    }
}
