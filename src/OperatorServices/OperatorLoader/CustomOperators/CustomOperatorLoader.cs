using Microsoft.Extensions.Logging;
using RightRule.System.Common.Operator.Models;

namespace RightRule.OperatorServices.OperatorLoader.CustomOperators;

internal sealed class CustomOperatorLoader : ICustomOperatorLoader
{
    private readonly ILogger<CustomOperatorLoader> _logger;

    public CustomOperatorLoader(ILogger<CustomOperatorLoader> logger)
    {
        _logger = logger;
    }

    public List<Operator> LoadOperators(string ConfigurationFilePath)
    {
        if (string.IsNullOrEmpty(ConfigurationFilePath) || !File.Exists(ConfigurationFilePath))
        {
            return new List<Operator>();
        }
        var operatorConfigs = GetOperatorConfigurations(ConfigurationFilePath);
        return LoadOperators(operatorConfigs,
            Path.GetDirectoryName(ConfigurationFilePath));
    }

    private List<Operator> LoadOperators(OperatorConfigs OperatorConfigurations, string? BaseDirectory)
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
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Custom Operator Loaded: {OperatorName}", op.Name);
                }
                operators.Add(op);
            }
        }
        return operators;
    }

    public Operator? LoadOperator(string PathToOperator)
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
