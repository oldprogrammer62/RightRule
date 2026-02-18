namespace RightRule.Operators;

public sealed class NotEqualsOperator : IOperator
{
    public string Name => "notequals";
    public bool Process(object left, object right) => !Equals(left, right);
}
