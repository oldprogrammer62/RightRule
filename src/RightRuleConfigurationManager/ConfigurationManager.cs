using RightRule.HelperFunctions;
using RightRule.Models;

namespace BeSoft.RightRuleConfigurationManager;

internal class ConfigurationManager
{
    public static RightRuleConfiguration GetConfiguration()
    {
        var enableDiagnostics = ConfigurationHelper.GetSetting("EnableDiagnostics")?.ToLower() == "true";
        var silentEvaluatorExceptions = ConfigurationHelper.GetSetting("SilentEvaluatorExceptions")?.ToLower() == "true";

        var RightRuleConfig = new RightRuleConfiguration(enableDiagnostics, silentEvaluatorExceptions);
        return RightRuleConfig;
    }
}
