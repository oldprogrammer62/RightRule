using System.Collections;

namespace Operator.Exists;

public class Exists : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["exists", "in", "foundin"];

    protected override async Task<bool> DoOperatorProcessAsync(object left, object right)
    {
        if (left == null && right == null)
        {
            return true;
        }
        if (left == null || right == null)
        {
            return false;
        }

        if (right is IList list)
        {
            return list.Contains(left);
        }

        if (right is ArrayList array)
        {
            foreach (var item in array)
            {
                if (Equals(item, left))
                {
                    return true;
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
                    return true;
                }
            }
        }
        return false;
    }
}
