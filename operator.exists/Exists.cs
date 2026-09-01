using RightRule.System.Common;
using System.Collections;

namespace Operator.Exists;

public class Exists : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["exists", "in", "foundin"];

    public Exists() : base()
    {
    }

    protected override async Task<OperatorResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, object? DecisionData, object? FalseDecisionData, bool Diagnostics)
    {
        if (left == null && right == null)
        {
            return OperatorResult.Failure("One or both operands are null.", DecisionData, FalseDecisionData);
        }
        if (left == null || right == null)
        {
            return OperatorResult.Failure("One or both operands are null.", DecisionData, FalseDecisionData);
        }

        if (right is IList list)
        {
            return OperatorResult.Success(list.Contains(left), DecisionData, FalseDecisionData);
        }

        if (right is ArrayList array)
        {
            foreach (var item in array)
            {
                if (Equals(item, left))
                {
                    return OperatorResult.Success(true, DecisionData, FalseDecisionData);
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
                    return OperatorResult.Success(true, DecisionData, FalseDecisionData);
                }
            }
        }
        return OperatorResult.Failure("Operand (right) is not a valid collection.", DecisionData, FalseDecisionData);
    }
}
