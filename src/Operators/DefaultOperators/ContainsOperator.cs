using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class ContainsOperator : Operator
{
    public ContainsOperator() : base(false)
    {
    }

    public override List<string> Identifiers { get;  } = ["contains"];

    protected override async Task<OperatorResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, object? DecisionData, bool Diagnostics)
    {
        if (left is null || right is null)
        {
            return OperatorResult.Failure("One or both operands are null.", DecisionData);
        }

        // String contains (case-insensitive)
        if (left is string ls && right is string rs)
        {
            return OperatorResult.Success(Contains(ls, rs), DecisionData);
        }

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return OperatorResult.Success(Contains(lstr, rstr), DecisionData);
    }

    private static bool Contains(string left, string right)
    {
        return left.Contains(right, StringComparison.OrdinalIgnoreCase);
    }
}
