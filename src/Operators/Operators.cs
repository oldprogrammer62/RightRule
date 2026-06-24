using RightRule.System.Common;

namespace RightRule.Operators;

internal class Operators
{
    internal static List<IOperator> RegisterOperators()
    {
        var operatorNamess = LoadOperators();
        if (operatorNamess == null || operatorNamess.Count == 0)
            throw new Exception("No operators found.");

        if (OperatorIdentifiersUnique(operatorNamess) == false)
            throw new Exception("Operator identifiers must be unique.");

        return operatorNamess;
    }

    internal static bool UnloadOperator(IOperator Operator)
    {
        if (Operator == null)
            return false;

        Operator.Identifiers.Clear(); // Clear identifiers to prevent future matches.
        return true; // Operator was found and removed.
    }

    private static bool OperatorIdentifiersUnique(List<IOperator> operators)
    {
        if (operators == null || operators.Count == 0)
            return true;

        // Flatten all identifiers, ignore null/whitespace, compare case-insensitively.
        var hasDuplicates = operators
            .Where(op => op?.Identifiers != null)
            .SelectMany(op => op.Identifiers)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .GroupBy(id => id, StringComparer.OrdinalIgnoreCase)
            .Any(g => g.Count() > 1);

        return !hasDuplicates;
    }

    private static List<IOperator> LoadOperators()
    {
        return
            [
            .. GetDefaultOperators(),
            .. RegisterCustomOperators()
            ];
    }

    private static List<IOperator> GetDefaultOperators()
    {
        return DefaultOperatorLoader.Load();
    }

    private static List<IOperator> RegisterCustomOperators()
    {
        return CustomOperatorLoader.Load();
    }
}
