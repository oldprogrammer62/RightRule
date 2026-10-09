using RightRule.RuleServices.RuleModels;

namespace RightRule.ConfigurationService;

internal class Helper
{
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
