using RightRule.RuleModels;

namespace RightRule.RuleRegistration;

internal class Helper
{
    internal static bool GetDataCheckSetting(RuleConfiguration? ruleConfig)
    {
        if (ruleConfig == null)
        {
            return RuleConfiguration.DefaultConfig().DataCheck;
        }
        return ruleConfig.DataCheck;
    }
}
