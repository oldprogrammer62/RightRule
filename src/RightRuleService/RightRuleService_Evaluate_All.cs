using RightRule.Models;
using RightRule.RuleApplication;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.System.Common;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    public async Task<List<RightRuleServiceResult<IRule>>> EvaluateAllAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await EvaluateSpecifiedRulesAsync(Context, RulesToReturn.All).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IRule>.Default(DefaultRule.DefaultResult())];
        }
        return BuildResult(rulesApplied);
    }

    public async Task<List<RightRuleServiceResult<IRule>>> EvaluateAllAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await EvaluateSpecifiedRulesAsync(Context, ReturnRules).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IRule>.Default(DefaultRule.DefaultResult())];
        }
        return BuildResult(rulesApplied);
    }

    public async Task<List<RightRuleServiceResult<IRule>>> EvaluateAllAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        var rulesApplied = await EvaluateSpecifiedRulesAsync(Context, DataName, Data, RulesToReturn.All).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IRule>.Default(DefaultRule.DefaultResult())];
        }
        return BuildResult(rulesApplied);
    }

    public async Task<List<RightRuleServiceResult<IRule>>> EvaluateAllAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        var rulesApplied = await EvaluateSpecifiedRulesAsync(Context, DataName, Data, ReturnRules).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IRule>.Default(DefaultRule.DefaultResult())];
        }
        return BuildResult(rulesApplied);
    }

    private async Task<List<IRule>> EvaluateSpecifiedRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        RegisterData(DataName, Data);
        List<IRule> appliedRules = await EvaluateSpecifiedRulesAsync(Context, ReturnRules).ConfigureAwait(false);
        DeRegisterData(DataName);
        return appliedRules;
    }

    private async Task<List<IRule>> EvaluateSpecifiedRulesAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registerRuleService.RegisteredRules.Rules == null || _registerRuleService.RegisteredRules.Rules.Count == 0)
        {
            return [ErrorRule.ErrorResult("Error: No rules available for application.")];
        }
        return await RuleApplicationService.ApplyRulesAsync(_interpreter, Context, _registerRuleService.RegisteredRules, ReturnRules).ConfigureAwait(false);
    }

    private static List<RightRuleServiceResult<IRule>> BuildResult(List<IRule> RulesApplied)
    {
        List<RightRuleServiceResult<IRule>> results = new List<RightRuleServiceResult<IRule>>();
        foreach (var ruleApplied in RulesApplied)
        {
            results.Add(RightRuleServiceResult<IRule>.Success(ruleApplied));
        }
        return results;
    }
}
