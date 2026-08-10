using RightRule.System.Common;
using System.Collections;

namespace Operator.Exists;

public class Exists : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["exists", "in", "foundin"];

    public Exists() : base()
    {
    }

    protected override async Task<EvaluationResult> DoOperatorProcessAsync(object left, object right, object? OptionalData)
    {
        if (left == null && right == null)
        {
            return EvaluationResult.Failure("One or both operands are null.");
        }
        if (left == null || right == null)
        {
            return EvaluationResult.Failure("One or both operands are null.");
        }

        if (right is IList list)
        {
            return EvaluationResult.Success(list.Contains(left));
        }

        if (right is ArrayList array)
        {
            foreach (var item in array)
            {
                if (Equals(item, left))
                {
                    return EvaluationResult.Success(true);
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
                    return EvaluationResult.Success(true);
                }
            }
        }
        return EvaluationResult.Failure("Operand (right) is not a valid collection.");
    }
}
