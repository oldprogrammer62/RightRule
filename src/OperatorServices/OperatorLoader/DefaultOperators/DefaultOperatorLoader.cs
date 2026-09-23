using RightRule.Operators.DefaultOperators;
using RightRule.System.Common;

namespace RightRule.OperatorServices.OperatorLoader.DefaultOperators;

internal class DefaultOperatorLoader
{
    internal static List<Operator> Load()
    {
        return new List<Operator>()
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
