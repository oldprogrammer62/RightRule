using RightRule.System.Common;

namespace RightRule.Operators.DefaultOperators;

public sealed class NotEqualsOperator : IOperator
{
    public List<string> Identifiers => ["notequals", "neq", "!=", "<>"];

    public bool Process(object left, object right)
    {
        return !Equals(left, right);
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
}
