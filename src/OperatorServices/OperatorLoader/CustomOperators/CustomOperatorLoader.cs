using RightRule.System.Common.Operator.Models;

namespace RightRule.OperatorServices.OperatorLoader.CustomOperators;

internal sealed class CustomOperatorLoader
{
    internal static List<Operator> LoadOperators(string ConfigurationFilePath)
    {
        if (string.IsNullOrEmpty(ConfigurationFilePath) || !File.Exists(ConfigurationFilePath))
        {
            return new List<Operator>();
        }
        var operatorConfigs = GetOperatorConfigurations(ConfigurationFilePath);
        return LoadOperators(operatorConfigs, 
            Path.GetDirectoryName(ConfigurationFilePath));
    }

    private static List<Operator> LoadOperators(OperatorConfigs OperatorConfigurations, string? BaseDirectory)
    {
        if (string.IsNullOrWhiteSpace(BaseDirectory))
        {
            BaseDirectory = AppContext.BaseDirectory;
        }

        if (OperatorConfigurations.Operators == null || OperatorConfigurations.Operators.Count == 0)
        {
            return new List<Operator>();
        }

        List<Operator> operators = new List<Operator>();
        foreach (var operatorConfiguration in OperatorConfigurations.Operators)
        {
            var op = LoadOperator(Path.Combine(BaseDirectory, operatorConfiguration.Path));
            if (op != null)
            {
                operators.Add(op);
            }
        }
        return operators;
    }

    internal static Operator? LoadOperator(string PathToOperator)
    {
        return ExternalAssemblyLoader.Load(PathToOperator);
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
