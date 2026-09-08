using RightRule.RuleModels;
using RightRule.RuleModels.RuleModels;

namespace RightRule.RegistrationServices;

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

    internal static bool GetDiagnosticsSetting(DefinedRule? ruleDefinition)
    {
        if (ruleDefinition == null)
        {
            return false;
        }
        return ruleDefinition.RuleConfiguration.Diagnostics;
    }
}
