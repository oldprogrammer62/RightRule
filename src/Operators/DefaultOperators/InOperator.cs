using RightRule.System.Common.Operator.Models;

namespace RightRule.Operators.DefaultOperators;

public sealed class InOperator : Operator
{
    public InOperator() : base(false)
    {
    }

    public override List<string> Aliases { get;  } = ["in"];

    protected override async Task<OperatorResult> ExecuteAsync(object Left, object Right, object? OptionalData)
    {
        if (Left is null || Right is null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }

        // String contains (case-insensitive)
        if (Left is string lStr && Right is Array rStrArray)
        {
            return OperatorResult.SuccessResult(
                this.Name,
                rStrArray.Cast<object?>().Any(value =>
                    string.Equals(value?.ToString(), lStr, StringComparison.OrdinalIgnoreCase)));
        }

        if (Left is int lInt && Right is Array rIntArray)
        {
            return OperatorResult.SuccessResult(
                this.Name,
                rIntArray.Cast<object?>().Any(value =>
                    int.Equals(value, lInt)));
        }

        if (Left is double lDbl && Right is Array rDblArray)
        {
            return OperatorResult.SuccessResult(
                this.Name,
                rDblArray.Cast<object?>().Any(value =>
                    int.Equals(value, lDbl)));
        }
        return OperatorResult.FailureResult(this.Name, "Invalid Array Datatypes");
    }
}
