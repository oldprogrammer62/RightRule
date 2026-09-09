namespace RightRule.OperatorServices;

internal class OperatorConfigs
{
    public required List<OperatorConfig> Operators { get; init; } = new List<OperatorConfig>();
}

internal class OperatorConfig
{
    public required string Name { get; init; } = string.Empty;
    public required string Path { get; init; } = string.Empty;
}
