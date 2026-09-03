using RightRule.RuleModels;

namespace RightRule.RuleRegistration.DefaultRuleRegistration;

internal class DefaultRuleRegister : IDefaultRuleRegister
{
    public DefaultRule _defaultRule { get; private set; }

    public DefaultRuleRegister()
    {
        _defaultRule = DefaultRule.DefaultResult();
    }

    public DefaultRule GetDefaultRule(string Context)
    {
        return _defaultRule;
    }

    public bool SetDefaultRule(DefaultRule rule)
    {
        if (rule == null)
        {
            return false;
        }
        _defaultRule = rule;
        return true;
    }
}
