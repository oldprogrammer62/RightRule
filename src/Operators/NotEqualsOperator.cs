using RightRule.System.Common;

namespace RightRule.Operators;

public sealed class NotEqualsOperator : IOperator
{
    public List<string> Identifiers => ["notequals", "neq", "!=", "<>"];

    public bool Process(object left, object right)
    {
        return !Equals(left, right);
    }
}
