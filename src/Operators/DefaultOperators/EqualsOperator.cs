using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class EqualsOperator : IOperator
{
    public List<string> Identifiers => ["equals", "eq", "=", "=="];
    public bool Process(object left, object right) => Equals(left, right);

    public void Dispose()
    {
        // Nothing to dispose
    }
}
