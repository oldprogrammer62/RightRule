using RightRule.HelperFunctions;
using RightRule.Models;
using RightRule.Operator.Loader;
using RightRule.System.Common;

namespace RightRule.Operators;

internal class CustomOperatorLoader
{
    internal static List<IOperator> Load()
    {
        string configPath = ConfigurationHelper.GetSetting("CustomOperatorConfigPath") ?? string.Empty;
        if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
        {
            return new List<IOperator>();
        }

        List<IOperator> operators = new List<IOperator>();
        var opConfigs = GetOperatorConfigs(configPath);

        if (opConfigs.Operators == null || opConfigs.Operators.Count == 0)
        {
            return operators;
        }

        foreach (var opConfig in opConfigs.Operators)
        {
            var op = Loader.Load(opConfig.Path);
            if (op != null)
            {
                operators.Add(op);
            }
        }
        return operators;
    }

    private static OperatorConfigs GetOperatorConfigs(string ConfigPath)
    {
        var configData = File.ReadAllText(ConfigPath);
        return Helpers.DeserialiseJson<OperatorConfigs>(configData);
    }
}
