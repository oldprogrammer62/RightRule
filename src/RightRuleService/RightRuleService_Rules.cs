using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RightRuleService;

internal sealed partial class RightRuleService : IRightRuleService
{
    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule)
    {
        if (Rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed");
        }
        var result = DefaultRuleRegister.SetDefaultRule(Rule);
        if (!result)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed", Rule);
        }
        return DefaultRuleRegistrationResult.Success("Default Rule registration success", Rule);
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        return DefaultRuleRegister.GetDefaultRule(Context);
    }
}
