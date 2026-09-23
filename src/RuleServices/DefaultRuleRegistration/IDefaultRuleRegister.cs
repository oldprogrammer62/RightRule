using RightRule.RuleServices.RuleModels;
using static RightRule.RuleServices.DefaultRuleRegistration.DefaultRuleRegister;

namespace RightRule.RuleServices.DefaultRuleRegistration;

internal interface IDefaultRuleRegister
{
    List<DefaultRulePerContext>? GetDefaultRules();
    bool SetDefaultRule(DefaultRule rule);
    DefaultRule GetDefaultRule(string Context);
}