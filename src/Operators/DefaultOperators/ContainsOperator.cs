using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class ContainsOperator : Operator
{
    public ContainsOperator() : base(false)
    {
    }

    public override List<string> Identifiers { get;  } = ["contains"];

    protected override async Task<EvaluationResult> DoOperatorProcessAsync(object left, object right, object? OperatorConfig)
    {
        if (left is null || right is null)
        {
            return EvaluationResult.Failure("One or both operands are null.");
        }

        // String contains (case-insensitive)
        if (left is string ls && right is string rs)
        {
            return EvaluationResult.Success(Contains(ls, rs));
        }

        // Fallback to string contains
        var lstr = left.ToString() ?? string.Empty;
        var rstr = right.ToString() ?? string.Empty;
        return EvaluationResult.Success(Contains(lstr, rstr));
    }

    private static bool Contains(string left, string right)
    {
        return left.Contains(right, StringComparison.OrdinalIgnoreCase);
    }
}
