using RightRule.RuleModels;

namespace RightRule.RuleRegistration.DefaultRuleRegistration;

public interface IDefaultRuleRegister
{
    bool SetDefaultRule(DefaultRule rule);
    DefaultRule GetDefaultRule(string Context);
}