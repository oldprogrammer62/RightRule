using RightRule.RuleApplication;
using RightRule.RuleModels;
using RightRule.System.Common;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    public async Task<List<AppliedRule>> EvaluateAllAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSpecifiedRulesAsync(Context, RulesToReturn.All).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateAllAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return await EvaluateSpecifiedRulesAsync(Context, ReturnRules).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateAllAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        return await EvaluateSpecifiedRulesAsync(Context, DataName, Data, RulesToReturn.All).ConfigureAwait(false);
    }

    public async Task<List<AppliedRule>> EvaluateAllAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        return await EvaluateSpecifiedRulesAsync(Context, DataName, Data, ReturnRules).ConfigureAwait(false);
    }

    private async Task<List<AppliedRule>> EvaluateSpecifiedRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        RegisterData(DataName, Data);
        List<AppliedRule> appliedRules = await EvaluateSpecifiedRulesAsync(Context, ReturnRules).ConfigureAwait(false);
        DeRegisterData(DataName);
        return appliedRules;
    }

    private async Task<List<AppliedRule>> EvaluateSpecifiedRulesAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registerRuleService.RegisteredRules.Rules == null || _registerRuleService.RegisteredRules.Rules.Count == 0)
        {
            return [AppliedRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRulesAsync(_interpreter, Context, _registeredDataSets, _registeredDataObjects, _registerRuleService.RegisteredRules, ReturnRules, _operatorsService.OperatorAliases).ConfigureAwait(false);
    }
}
