using RightRule.ConfigurationService;
using RightRule.OperatorServices;
using RightRule.System.Common;

namespace RightRule.OperatorServices.OperatorLoader.CustomOperators;

internal class CustomOperatorLoader
{
    internal static List<Operator> Load()
    {
        string configPath = GetConfigPath();
        if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
        {
            return new List<Operator>();
        }

        List<Operator> operators = new List<Operator>();
        var opConfigs = GetOperatorConfigs(configPath);
        if (opConfigs.Operators == null || opConfigs.Operators.Count == 0)
        {
            return operators;
        }

        foreach (var opConfig in opConfigs.Operators)
        {
            var op = ExternalAssemblyLoader.Load(opConfig.Path);
            if (op != null)
            {
                operators.Add(op);
            }
        }
        return operators;
    }

    private static string GetConfigPath()
    {
        try
        {
            return ConfigurationHelper.GetSetting("CustomOperatorConfigPath") ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static OperatorConfigs GetOperatorConfigs(string ConfigPath)
    {
        try
        {
            var configData = File.ReadAllText(ConfigPath);
            return Helpers.DeserialiseJson<OperatorConfigs>(configData);
        }
        catch
        {
            return new OperatorConfigs { Operators = new List<OperatorConfig>() };
        }
    }
}
