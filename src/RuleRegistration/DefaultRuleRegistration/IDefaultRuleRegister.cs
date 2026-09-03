using RightRule.RuleModels.BaseModels;

namespace RightRule.RuleRegistration.DefaultRuleRegistration;

public interface IDefaultRuleRegister
{
    bool SetDefaultRule(RuleDefault rule);
    RuleDefault GetDefaultRule();
}