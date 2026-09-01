using RightRule.System.Common;
using System.Collections;

namespace Operator.Exists;

public class Exists : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["exists", "in", "foundin"];

    public Exists() : base()
    {
    }

    protected override async Task<OperatorResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, bool Diagnostics)
    {
        if (left == null && right == null)
        {
            return OperatorResult.Failure(this.Name, "One or both operands are null.");
        }
        if (left == null || right == null)
        {
            return OperatorResult.Failure(this.Name, "One or both operands are null.");
        }

        if (right is IList list)
        {
            return OperatorResult.Success(this.Name, list.Contains(left));
        }

        if (right is ArrayList array)
        {
            foreach (var item in array)
            {
                if (Equals(item, left))
                {
                    return OperatorResult.Success(this.Name, true);
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
                    return OperatorResult.Success(this.Name, true);
                }
            }
        }
        return OperatorResult.Failure(this.Name, "Operand (right) is not a valid collection.");
    }
}
