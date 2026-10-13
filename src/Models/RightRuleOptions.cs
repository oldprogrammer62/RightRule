using System.Configuration;

namespace RightRule.Models;

public sealed class RightRuleOptions
{
    public bool LoadDefaultOperators { get; set; } = true;
    public bool LoadCustomOperators { get; set; } = true;
    public bool DataCheck { get; set; } = false;
    public bool Diagnostics { get; set; } = false;

    public string CustomOperatorLibraryPath { get; set; } = RightRuleLibraryDefaults.CustomOperatorLibraryPath;
}

internal static class RightRuleLibraryDefaults
{
    public static readonly string CustomOperatorLibraryPath = ReadCustomOperatorLibraryPath();

    private static string ReadCustomOperatorLibraryPath()
    {
        var assemblyPath = typeof(RightRuleLibraryDefaults).Assembly.Location;
        var configPath = assemblyPath + ".config";

        if (!File.Exists(configPath))
            return string.Empty;

        var map = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
        var config = ConfigurationManager.OpenMappedExeConfiguration(
            map, ConfigurationUserLevel.None);

        var configuredPath = config.AppSettings.Settings["CustomOperatorConfigPath"]?.Value;
        if (string.IsNullOrWhiteSpace(configuredPath))
            return string.Empty;

        var assemblyDirectory = Path.GetDirectoryName(assemblyPath)!;
        return Path.GetFullPath(configuredPath, assemblyDirectory);
    }
}
