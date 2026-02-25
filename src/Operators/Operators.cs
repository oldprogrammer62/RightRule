using RightRule.Models;
using RightRule.Operator.Loader;
using RightRule.System.Common;

namespace RightRule.Operators;

internal class Operators
{
    internal static List<IOperator> GetOperators()
    {
        var operators = FetchOperators();
        if (operators == null || operators.Count == 0)
            throw new Exception("No operators found.");

        if (OperatorIdentifiersUnique(operators) == false)
            throw new Exception("Operator identifiers must be unique.");

        return operators;
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

    private static List<IOperator> FetchOperators()
    {
        List<IOperator> operators = new List<IOperator>();

        operators.AddRange(GetDefaultOperators());
        operators.AddRange(RegisterConfiguredOperators());

        return operators;
    }

    private static List<IOperator> GetDefaultOperators()
    {
        return new List<IOperator>
        {
            new EqualsOperator(),
            new NotEqualsOperator(),
            new ContainsOperator()
        };
    }

    private static List<IOperator> RegisterConfiguredOperators()
    {
        List<IOperator> operators = new List<IOperator>();
        var opConfigs = GetOperatorConfigs();

        if (opConfigs.Operators == null || opConfigs.Operators.Count == 0)
        {
            return operators;
        }

        foreach (var opConfig in opConfigs.Operators)
        {
            var op = Loader.Load(opConfig.Path);
            operators.Add(op);
        }
        return operators;
    }

    private static OperatorConfigs GetOperatorConfigs()
    {
        // Load config from file
        string configPath = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\operatorconfig.json";

        var config = File.ReadAllText(configPath);

        var jsonSerializer = new Newtonsoft.Json.JsonSerializer();

        using (var jsonReader = new Newtonsoft.Json.JsonTextReader(new StringReader(config)))
        {
            var operatorConfigs = jsonSerializer.Deserialize<OperatorConfigs>(jsonReader);
            if (operatorConfigs == null)
                throw new Exception("Failed to load operator configs.");

            return operatorConfigs;
        }
    }
}
