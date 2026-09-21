using RightRule.System.Common;
using System.Reflection;

namespace RightRule.Loaders.OperatorLoader.CustomerOperators;

public class ExternalAssemblyLoader
{
    public static Operator? Load(string PathToFunction)
    {
        var _operator = Load(PathToFunction, true);
        return _operator;
    }

    internal static Operator? Load(string PathToFunction, bool IsCollectible)
    {
        if (string.IsNullOrWhiteSpace(PathToFunction) || !File.Exists(PathToFunction))
            throw new FileNotFoundException("Operator assembly not found", PathToFunction);

        var loader = new ExternalOperatorLoader(PathToFunction, IsCollectible);

        try
        {
            Assembly assem = loader.LoadFromAssemblyPath(PathToFunction);

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
