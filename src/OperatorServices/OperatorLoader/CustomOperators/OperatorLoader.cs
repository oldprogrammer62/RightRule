namespace RightRule.OperatorServices.OperatorLoader.CustomOperators;

internal class OperatorLoader
{
    public static object? Load(Type OpType)
    {
        return Activator.CreateInstance(OpType);
    }
}
