using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;

namespace RightRule.RuleApplication
{
    internal interface IApplyAnyRulesService
    {
        Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType);
    }
}