using RightRule.System.Common;
using System.Reflection;

namespace RightRule.Operator.Loader;

public class Loader
{
    public static IOperator Load(string pathToFunction)
    {
        var _operator = Load(pathToFunction, true);
        return _operator;
    }

    internal static IOperator Load(string pathToFunction, bool b)
    {
        if (string.IsNullOrWhiteSpace(pathToFunction) || !File.Exists(pathToFunction))
            throw new FileNotFoundException("Operator assembly not found", pathToFunction);

        var opLoader = new OperatorLoader(pathToFunction, true);

        try
        {
            Assembly assem = opLoader.LoadFromAssemblyPath(pathToFunction);
            Type functionType = assem.ExportedTypes.Single(t =>
            typeof(IOperator).IsAssignableFrom(t));

            var instance = Activator.CreateInstance(functionType);
            if (instance is IOperator op)
                return op;

            throw new InvalidOperationException("Failed to create operator instance from assembly.");
        }
        catch { }

        return null;
    }
}
