using RightRule.Models;
using RightRule.RuleModels;

namespace RightRule.RuleRegistration;

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

    internal static bool GetDiagnosticsSetting(RuleDefinition? ruleDefinition)
    {
        if (ruleDefinition == null)
        {
            return false;
        }
        return ruleDefinition.RuleConfiguration.Diagnostics;
    }
}
