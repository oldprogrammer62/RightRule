using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RuleApplication
{
    internal interface IRuleApplicationService
    {
        Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType);
    }
}