namespace RightRule.Models;

internal class OperatorConfigs
{
    public required List<OperatorConfig> Operators { get; set; } = new List<OperatorConfig>();
}

internal class OperatorConfig
{
    public required string Name { get; set; } = string.Empty;
    public required string Path { get; set; } = string.Empty;
}
