using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public async Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyRulesAsync(Context, EvaluationTypeEnum.FIRST).ConfigureAwait(false);
        if (rulesApplied == null || rulesApplied.Count == 0)
        {
            return RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context));
        }
        var result = rulesApplied.FirstOrDefault();
        if (result != null)
        {
            return result;
        }
        return RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context));
    }

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await RuleApplicationService.ApplyRulesAsync(Context, evaluationType).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
        }
        return BuildResult(rulesApplied);
    }
}
