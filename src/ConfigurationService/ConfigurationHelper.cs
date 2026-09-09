using System.Configuration;
using System.Reflection;

namespace RightRule.ConfigurationService;

internal class ConfigurationHelper
{
    private static readonly Configuration _config;

    static ConfigurationHelper()
    {
        var dllPath = Assembly.GetExecutingAssembly().Location;
        _config = ConfigurationManager.OpenExeConfiguration(dllPath);
    }

    public static string GetSetting(string key)
    {
        return _config.AppSettings.Settings[key]?.Value ?? string.Empty;
    }
}
