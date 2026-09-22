using RightRule.Models.RuleModels;
using static RightRule.RegistrationServices.DefaultRuleRegistration.DefaultRuleRegister;

namespace RightRule.RegistrationServices.DefaultRuleRegistration;

internal interface IDefaultRuleRegister
{
    List<DefaultRulePerContext>? GetDefaultRules();
    bool SetDefaultRule(DefaultRule rule);
    DefaultRule GetDefaultRule(string Context);
}