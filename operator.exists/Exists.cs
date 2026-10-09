using RightRule.System.Common.Operator.Models;
using System.Collections;

namespace Exists;

public class Exists : Operator
{
    public override List<string> Aliases => ["exists", "in", "foundin"];

    public Exists() : base()
    {
    }

    protected override async Task<OperatorResult> ExecuteAsync(object left, object right, object? OptionalData)
    {
        if (left == null && right == null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }
        if (left == null || right == null)
        {
            return OperatorResult.FailureResult(this.Name, "One or both operands are null.");
        }

        if (right is IList list)
        {
            return OperatorResult.SuccessResult(this.Name, list.Contains(left));
        }

        if (right is ArrayList array)
        {
            foreach (var item in array)
            {
                if (Equals(item, left))
                {
                    return OperatorResult.SuccessResult(this.Name, true);
                }
            }
        }

        if (right is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                var sLeft = left.ToString();
                var sItem = item.ToString();
                if (Equals(sItem, sLeft))
                {
                    return OperatorResult.SuccessResult(this.Name, true);
                }
            }
        }
        return OperatorResult.FailureResult(this.Name, "Operand (right) is not a valid collection.");
    }
}
