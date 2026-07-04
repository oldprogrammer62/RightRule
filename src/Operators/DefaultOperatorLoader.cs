using RightRule.Operators.DefaultOperators;
using RightRule.System.Common;

namespace RightRule.Operators;

internal class DefaultOperatorLoader
{
    internal static List<IOperator> Load()
    {
        return new List<IOperator>
        {
            new EqualsOperator(),
            new NotEqualsOperator(),
            new ContainsOperator(),
            new DoesNotContainOperator(),
            new LessThanOperator(),
            new GreaterThanOrEqualsOperator()
        };
    }
}
