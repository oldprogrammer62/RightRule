using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;

namespace RightRule.RuleRegistration.DefaultRuleRegistration;

internal class DefaultRuleRegister : IDefaultRuleRegister
{
    public RuleDefault _defaultRule { get; private set; }

    public DefaultRuleRegister()
    {
        _defaultRule = DefaultRule.DefaultResult();
    }

    public RuleDefault GetDefaultRule()
    {
        return _defaultRule;
    }

    public bool SetDefaultRule(RuleDefault rule)
    {
        if (rule == null)
        {
            return false;
        }
        _defaultRule = rule;
        return true;
    }
}
