using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RuleServices.RuleApplication
{
    internal interface IRuleApplicationService
    {
        Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType);
    }
}