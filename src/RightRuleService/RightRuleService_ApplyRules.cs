using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public async Task<RightRuleServiceResult<IProcessedRule>> ApplyRules_SingleResultAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyRules_MultipleResultAsync(Context, EvaluationType).ConfigureAwait(false);
        if (rulesApplied == null || rulesApplied.Count == 0)
        {
            return BuildDefaultResult(Context);
        }
        var result = rulesApplied.FirstOrDefault();
        if (result != null)
        {
            return result;
        }
        return BuildDefaultResult(Context);
    }

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRules_MultipleResultAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await RuleApplicationService.ApplyRulesAsync(Context, EvaluationType).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [BuildDefaultResult(Context)];
        }
        return BuildResult(rulesApplied);
    }
}
