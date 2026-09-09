using RightRule.Models;
using RightRule.Models.RuleModels;

namespace RightRule.ConfigurationService;

internal class Helper
{
    internal static bool GetDataCheckSetting(RightRuleConfiguration? ruleConfig)
    {
        if (ruleConfig == null)
        {
            return RightRuleConfiguration.DefaultConfig().DataCheck;
        }
        return ruleConfig.DataCheck;
    }

    internal static bool GetDiagnosticsSetting(RightRuleConfiguration? ruleConfig)
    {
        if (ruleConfig == null)
        {
            return RightRuleConfiguration.DefaultConfig().Diagnostics;
        }
        return ruleConfig.Diagnostics;
    }

    internal static bool GetDiagnosticsSetting(DefinedRule? ruleDefinition)
    {
        if (ruleDefinition == null)
        {
            return false;
        }
        return ruleDefinition.RuleConfiguration.Diagnostics;
    }
}
