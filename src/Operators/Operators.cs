namespace RightRule.Operators;

internal class Operators
{
    private static readonly List<IOperator> _operators = new List<IOperator>();

    internal static List<IOperator> GetOperators()
    {
        if (_operators.Count == 0)
        {
            RegisterOperators();
        }
        return _operators;
    }

    private static void RegisterOperators()
    {
        _operators.Add(new EqualsOperator());
        _operators.Add(new NotEqualsOperator());
        _operators.Add(new ContainsOperator());
    }
}
