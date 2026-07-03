using RightRule.System.Common;
using System.Collections;

namespace Operator.Exists;

public class Exists : IOperator
{
    public List<string> Identifiers => ["exists", "in", "foundin"];

    /// <summary>
    /// Determines whether the element specified in left is found in the List specified in right.
    /// </summary>
    /// <param name="left">An element to be checked for existence in the list.</param>
    /// <param name="right">A List of elements.</param>
    /// <returns></returns>
    public bool Process(object left, object right)
    {
        if (left == null && right == null)
        {
            return true;
        }
        if (left == null || right == null)
        {
            return true;
        }

        // Check if right is a List
        if (right is IList list)
        {
            return list.Contains(left);
        }

        if (right is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                var sLeft = left.ToString();
                var sItem = item.ToString();
                if (Equals(item, left))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
}
