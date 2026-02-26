using RightRule.Models;
using RightRule.Operator.Loader;
using RightRule.System.Common;

namespace RightRule.Operators;

internal class CustomOperatorLoader
{
    internal static List<IOperator> Load()
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
        string configPath = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\customoperatorconfig.json";

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
