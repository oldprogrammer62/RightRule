using Microsoft.Extensions.Configuration;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace RightRule.Models;

public sealed class RightRuleOptions
{
    public bool DataCheck { get; set; } = false;
    public bool Diagnostics { get; set; } = true;
    public bool LoadCustomOperators { get; set; } = true;
    public bool LoadDefaultOperators { get; set; } = true;

    public string CustomOperatorLibraryPath { get; set; }

    public RightRuleOptions()
    {
        DataCheck = ReadLibraryBoolean("DataCheck") ?? false;
        Diagnostics = ReadLibraryBoolean("Diagnostics") ?? false;
        LoadCustomOperators = ReadLibraryBoolean("LoadCustomOperators") ?? true;
        LoadDefaultOperators = ReadLibraryBoolean("LoadDefaultOperators") ?? true;

        CustomOperatorLibraryPath = RightRuleLibraryDefaults.CustomOperatorLibraryPath;
    }

    public RightRuleOptions(IConfiguration configuration) : this()
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection("RightRule");

        LoadDefaultOperators = ReadBoolean(section["LoadDefaultOperators"], LoadDefaultOperators);
        LoadCustomOperators = ReadBoolean(section["LoadCustomOperators"], LoadCustomOperators);
        DataCheck = ReadBoolean(section["DataCheck"], DataCheck);
        Diagnostics = ReadBoolean(section["Diagnostics"], Diagnostics);

        if (section["CustomOperatorLibraryPath"] is { } path)
            CustomOperatorLibraryPath = path;
    }

    private static bool ReadBoolean(string? value, bool fallback) =>
        string.IsNullOrWhiteSpace(value) ? fallback : bool.Parse(value);

    private static bool? ReadLibraryBoolean(string key)
    {
        var value = RightRuleLibraryDefaults.ReadSetting(key);
        return string.IsNullOrWhiteSpace(value) ? null : bool.Parse(value);
    }
}

internal static class RightRuleLibraryDefaults
{
    private static readonly Configuration? LibraryConfiguration = ReadConfiguration();

    public static readonly string CustomOperatorLibraryPath = ReadCustomOperatorLibraryPath();

    public static string? ReadSetting(string key) =>
        LibraryConfiguration?.AppSettings.Settings[key]?.Value;

    private static Configuration? ReadConfiguration()
    {
        var assemblyPath = typeof(RightRuleLibraryDefaults).Assembly.Location;
        var configPath = assemblyPath + ".config";

        if (!File.Exists(configPath))
            return null;

        var map = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
        return ConfigurationManager.OpenMappedExeConfiguration(
            map, ConfigurationUserLevel.None);
    }

    private static string ReadCustomOperatorLibraryPath()
    {
        var configuredPath = ReadSetting("CustomOperatorConfigPath");
        if (string.IsNullOrWhiteSpace(configuredPath))
            return string.Empty;

        var assemblyDirectory =
            Path.GetDirectoryName(typeof(RightRuleLibraryDefaults).Assembly.Location)!;

        return Path.GetFullPath(configuredPath, assemblyDirectory);
    }
}

//internal static class RightRuleLibraryDefaults
//{
//    public static readonly string CustomOperatorLibraryPath = ReadCustomOperatorLibraryPath();

//    private static string ReadCustomOperatorLibraryPath()
//    {
//        var assemblyPath = typeof(RightRuleLibraryDefaults).Assembly.Location;
//        var configPath = assemblyPath + ".config";

//        if (!File.Exists(configPath))
//            return string.Empty;

//        var map = new ExeConfigurationFileMap { ExeConfigFilename = configPath };
//        var config = ConfigurationManager.OpenMappedExeConfiguration(
//            map, ConfigurationUserLevel.None);

//        var configuredPath = config.AppSettings.Settings["CustomOperatorConfigPath"]?.Value;
//        if (string.IsNullOrWhiteSpace(configuredPath))
//            return string.Empty;

//        var assemblyDirectory = Path.GetDirectoryName(assemblyPath)!;
//        return Path.GetFullPath(configuredPath, assemblyDirectory);
//    }
// }
