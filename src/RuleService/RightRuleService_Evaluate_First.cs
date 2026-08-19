using RightRule.RuleApplication;
using RightRule.RuleModels;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    public async Task<List<AppliedRule>> EvaluateFirstRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSingleRuleAsync(Context).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateFirstRuleAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        RegisterData(DataName, Data);
        List<AppliedRule> appliedRules = await EvaluateSingleRuleAsync(Context).ConfigureAwait(false);
        DeRegisterData(DataName);
        return appliedRules;
    }

    private async Task<List<AppliedRule>> EvaluateSingleRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registerRuleService.RegisteredRules.Rules == null || _registerRuleService.RegisteredRules.Rules.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRuleAsync(_evaluator, Context, _registeredDataSets, _registeredDataObjects, _registerRuleService.RegisteredRules, _operatorsService.OperatorAliases).ConfigureAwait(false);
    }
}
