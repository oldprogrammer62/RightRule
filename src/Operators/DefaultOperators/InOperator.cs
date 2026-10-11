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

        if (Right is Array values)
        {
            var found = values.Cast<object?>().Any(value =>
        value is string item && Left is string leftString
            ? string.Equals(item, leftString, StringComparison.OrdinalIgnoreCase)
            : object.Equals(value, Left));

            return OperatorResult.SuccessResult(this.Name, found);
        }
        return OperatorResult.FailureResult(this.Name, "Invalid Array Datatypes");
    }
}
