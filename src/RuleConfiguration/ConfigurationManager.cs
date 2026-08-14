using RightRule.HelperFunctions;
using RightRule.Models;

namespace RightRule.RuleConfiguration;

internal class ConfigurationManager
{
    internal static RightRuleConfiguration GetConfiguration()
    {
        var enableDiagnostics = GetSetting("EnableDiagnostics");

        var RightRuleConfig = new RightRuleConfiguration(enableDiagnostics);
        return RightRuleConfig;
    }

    private static bool GetSetting(string Setting)
    {
        var sSetting = ConfigurationHelper.GetSetting(Setting)?.ToLower();
        if (string.IsNullOrWhiteSpace(sSetting))
        {
            return false;
        }
        if (sSetting == "true") { return true; }
        return false;
    }
}
