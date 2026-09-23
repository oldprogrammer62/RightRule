using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RuleServices.RuleApplication
{
    internal interface IApplyFirstRuleService
    {
        Task<IProcessedRule> ApplyRuleAsync(string Context);
    }
}