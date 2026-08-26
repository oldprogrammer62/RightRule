using RightRule.Models;

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
}
