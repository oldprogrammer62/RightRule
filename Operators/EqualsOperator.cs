namespace RightRule.Operators;

public sealed class EqualsOperator : IOperator
{
    public string Name => "equals";
    public bool Process(object left, object right) => Equals(left, right);
}
