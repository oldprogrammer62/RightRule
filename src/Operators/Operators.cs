using RightRule.System.Common;
using RightRule.Operator.Loader;
using RightRule.Models;

namespace RightRule.Operators;

internal class Operators
{
    private static readonly List<IOperator> _operators = new List<IOperator>();

    internal static List<IOperator> GetOperators()
    {
        if (_operators.Count == 0)
        {
            RegisterOperators();
            LoadOperators();
        }
        return _operators;
    }

    private static void RegisterOperators()
    {
        _operators.Add(new EqualsOperator());
        _operators.Add(new NotEqualsOperator());
        _operators.Add(new ContainsOperator());
    }

    private static void LoadOperators()
    {
        var opConfigs = GetOperatorConfigs();

        if (opConfigs.Operators == null || opConfigs.Operators.Count == 0)
        {
            return;
        }

        foreach (var opConfig in opConfigs.Operators)
        {
            var op = Loader.Load(opConfig.Path);
            _operators.Add(op);
        }
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
