using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleBuilders.DefaultRuleBuilder;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    public IDefaultNameStep BuildDefaultRule()
    {
        return DefaultRuleBuilder.Create();
    }

    public DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule)
    {
        if (Rule == null)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed");
        }
        var result = _defaultRuleRegister.SetDefaultRule(Rule);
        if (!result)
        {
            return DefaultRuleRegistrationResult.Failed("Default Rule registration failed", Rule);
        }
        return DefaultRuleRegistrationResult.Success("Default Rule registration success", Rule);
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        return _defaultRuleRegister.GetDefaultRule(Context);
    }
}
