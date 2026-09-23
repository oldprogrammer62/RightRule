using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RuleApplication
{
    internal interface IApplyFirstRuleService
    {
        Task<IProcessedRule> ApplyRuleAsync(string Context);
    }
}