namespace RightRule.Operators;

public sealed class EqualsOperator : IOperator
{
    public List<string> Names => ["equals", "eq", "=", "=="];
    public bool Process(object left, object right) => Equals(left, right);
}
