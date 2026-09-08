using RightRule.RuleModels.RuleModels;

namespace RightRule.RegistrationServices.DefaultRuleRegistration;

public interface IDefaultRuleRegister
{
    bool SetDefaultRule(DefaultRule rule);
    DefaultRule GetDefaultRule(string Context);
}