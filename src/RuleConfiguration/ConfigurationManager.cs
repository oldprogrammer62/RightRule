using RightRule.HelperFunctions;
using RightRule.Models;

namespace RightRule.RuleConfiguration;

internal class ConfigurationManager
{
    internal static RightRuleConfiguration GetConfiguration()
    {
        var enableDiagnostics = ConfigurationHelper.GetSetting("EnableDiagnostics")?.ToLower() == "true";

        var RightRuleConfig = new RightRuleConfiguration(enableDiagnostics);
        return RightRuleConfig;
    }
}
