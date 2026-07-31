using RightRule.System.Common;
using System.Reflection;

namespace RightRule.OperatorLoader;

public class Loader
{
    public static Operator? Load(string pathToFunction)
    {
        var _operator = Load(pathToFunction, true);
        return _operator;
    }

    internal static Operator? Load(string pathToFunction, bool collectible)
    {
        if (string.IsNullOrWhiteSpace(pathToFunction) || !File.Exists(pathToFunction))
            throw new FileNotFoundException("Operator assembly not found", pathToFunction);

        var loader = new OperatorLoader(pathToFunction, collectible);

        try
        {
            Assembly assem = loader.LoadFromAssemblyPath(pathToFunction);

            Type? functionType = assem.ExportedTypes.FirstOrDefault(t => typeof(Operator).IsAssignableFrom(t));
            if (functionType == null)
            {
                throw new InvalidOperationException("No valid implementation of Operator found.");
            }

            var instance = Activator.CreateInstance(functionType);
            if (instance is Operator op)
            {
                return op;
            }

            throw new InvalidOperationException("Failed to create operator instance from assembly.");
        }
        catch
        {
            throw;
        }
    }
}
