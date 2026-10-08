using RightRule.System.Common;

namespace RightRule.OperatorServices.OperatorLoader.CustomOperators;

internal class CustomOperatorLoader
{
    internal static List<Operator> LoadOperators(string ConfigurationFilePath)
    {
        if (string.IsNullOrEmpty(ConfigurationFilePath) || !File.Exists(ConfigurationFilePath))
        {
            return new List<Operator>();
        }
        var operatorConfigs = GetOperatorConfigurations(ConfigurationFilePath);
        return LoadOperators(operatorConfigs);
    }

    private static List<Operator> LoadOperators(OperatorConfigs OperatorConfigurations)
    {
        if (OperatorConfigurations.Operators == null || OperatorConfigurations.Operators.Count == 0)
        {
            return new List<Operator>();
        }

        List<Operator> operators = new List<Operator>();
        foreach (var operatorConfiguration in OperatorConfigurations.Operators)
        {
            var op = LoadOperator(operatorConfiguration.Path);
            if (op != null)
            {
                operators.Add(op);
            }
        }
        return operators;
    }

    internal static Operator? LoadOperator(string OperatorConfiguration)
    {
        return ExternalAssemblyLoader.Load(OperatorConfiguration);
    }

    private static OperatorConfigs GetOperatorConfigurations(string ConfigurationFilePath)
    {
        try
        {
            var configData = File.ReadAllText(ConfigurationFilePath);
            return Helpers.DeserialiseJson<OperatorConfigs>(configData);
        }
        catch
        {
            return new OperatorConfigs { Operators = new List<OperatorConfig>() };
        }
    }
}
