using RightRule.System.Common.Operator.Models;
using System.Reflection;

namespace RightRule.OperatorServices.OperatorLoader.CustomOperators;

internal class ExternalAssemblyLoader
{
    internal static Operator? Load(string PathToFunction)
    {
        var _operator = Load(PathToFunction, true);
        return _operator;
    }

    private static Operator? Load(string PathToOperator, bool IsCollectible)
    {
        if (string.IsNullOrWhiteSpace(PathToOperator) || !File.Exists(PathToOperator))
            throw new FileNotFoundException("Operator assembly not found", PathToOperator);

        var loader = new ExternalOperatorLoader(PathToOperator, IsCollectible);

        try
        {
            Assembly operatorAssembly = loader.LoadFromAssemblyPath(PathToOperator);

            Type? operatorType = operatorAssembly.ExportedTypes.FirstOrDefault(t => typeof(Operator).IsAssignableFrom(t));
            if (operatorType == null)
            {
                throw new InvalidOperationException("No valid implementation of Operator found.");
            }

            var operatorInstance = LoadOperator(operatorType);
            if (operatorInstance is Operator op)
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

    private static object? LoadOperator(Type OperatorType)
    {
        return Activator.CreateInstance(OperatorType);
    }
}
