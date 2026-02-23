using System.Common;

namespace RightRule.Operators;

public sealed class NotEqualsOperator : IOperator
{
    public List<string> Names => ["notequals", "neq", "!=", "<>"];
    public bool Process(object left, object right) => !Equals(left, right);
}
