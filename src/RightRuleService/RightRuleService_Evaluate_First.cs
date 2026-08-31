using RightRule.Models;
using RightRule.RuleApplication;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    public async Task<RightRuleServiceResult<IRule>> EvaluateFirstAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var applied = await EvaluateFirstMatchAsync(Context).ConfigureAwait(false);
        if (applied == null)
        {
            return RightRuleServiceResult<IRule>.Default(DefaultRule.DefaultResult());
        }
        return RightRuleServiceResult<IRule>.Success(applied);
    }

    public async Task<RightRuleServiceResult<IRule>> EvaluateFirstMatchWithDataAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        RegisterData(DataName, Data);
        IRule appliedRule = await EvaluateFirstMatchAsync(Context).ConfigureAwait(false);
        DeRegisterData(DataName);

        var svc = RightRuleServiceResult<IRule>.Success(appliedRule);
        return svc;
    }

    private async Task<IRule> EvaluateFirstMatchAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registerRuleService.RegisteredRules.Rules == null || _registerRuleService.RegisteredRules.Rules.Count == 0)
        {
            return ErrorRule.ErrorResult("Error: No rules available for application.");
        }
        return await RuleApplicationService.EvaluateFirstAsync(_interpreter, Context, _registerRuleService.RegisteredRules).ConfigureAwait(false);
    }
}
