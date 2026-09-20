using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;

namespace RightRule.RuleApplication
{
    internal interface IApplyFirstRuleService
    {
        Task<IProcessedRule> ApplyRuleAsync(string Context);
    }
}