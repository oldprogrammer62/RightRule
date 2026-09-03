using RightRule.Models;
using RightRule.RuleApplication;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.System.Common;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    public async Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyRulesAsync(Context, RulesToReturn.First).ConfigureAwait(false);
        if (rulesApplied == null || rulesApplied.Count == 0)
        {
            return RightRuleServiceResult<IProcessedRule>.Default(GetDefaultRule());
        }
        var result = rulesApplied.FirstOrDefault();
        if (result != null)
        {
            return result;
        }
        return RightRuleServiceResult<IProcessedRule>.Default(GetDefaultRule());
    }

    public async Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        var rulesApplied = await ApplyRulesAsync(Context, DataName, Data, RulesToReturn.First).ConfigureAwait(false);
        if (rulesApplied == null || rulesApplied.Count == 0)
        {
            return RightRuleServiceResult<IProcessedRule>.Default(GetDefaultRule());
        }
        var result = rulesApplied.FirstOrDefault();
        if (result != null)
        {
            return result;
        }
        return RightRuleServiceResult<IProcessedRule>.Default(GetDefaultRule());
    }

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, RulesToReturn rulesToReturn)
    {
        return rulesToReturn switch
        {
            RulesToReturn.First => await ApplyFirstMatchAsync(Context, rulesToReturn).ConfigureAwait(false),
            RulesToReturn.All => await ApplyAllMatchAsync(Context, rulesToReturn).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(rulesToReturn), $"Invalid value: {rulesToReturn}")
        };
    }

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        return rulesToReturn switch
        {
            RulesToReturn.First => [await ApplyFirstMatchAsync(Context, DataName, Data).ConfigureAwait(false)],
            RulesToReturn.All => await ApplyAllMatchAsync(Context, DataName, Data).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(rulesToReturn), $"Invalid value: {rulesToReturn}")
        };
    }

    private async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyFirstMatchAsync(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyMatchAsync_RAS(Context, rulesToReturn).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IProcessedRule>.Default(GetDefaultRule())];
        }
        return BuildResult(rulesApplied);
    }

    private async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyAllMatchAsync(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyMatchAsync_RAS(Context, rulesToReturn).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IProcessedRule>.Default(GetDefaultRule())];
        }
        return BuildResult(rulesApplied);
    }

    private async Task<RightRuleServiceResult<IProcessedRule>> ApplyFirstMatchAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredRuleService.RegisteredRules.Rules == null || _registeredRuleService.RegisteredRules.Rules.Count == 0)
        {
            return RightRuleServiceResult<IProcessedRule>.Default(GetDefaultRule());
        }
        RegisterData(DataName, Data);
        var rulesApplied = await ApplyMatchAsync_RAS(Context, RulesToReturn.First).ConfigureAwait(false);
        DeRegisterData(DataName);

        var ruleApplied = rulesApplied.FirstOrDefault() ?? GetDefaultRule();
        return RightRuleServiceResult<IProcessedRule>.Success(ruleApplied);
    }

    private async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyAllMatchAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredRuleService.RegisteredRules.Rules == null || _registeredRuleService.RegisteredRules.Rules.Count == 0)
        {
            return [RightRuleServiceResult<IProcessedRule>.Default(GetDefaultRule())];
        }
        RegisterData(DataName, Data);
        var ruleApplied = await ApplyMatchAsync_RAS(Context, RulesToReturn.All).ConfigureAwait(false);
        DeRegisterData(DataName);

        return BuildResult(ruleApplied);
    }

    // *****************************************
    // *** Calls to RULE APPLICATION SERVICE ***
    // *****************************************

    private async Task<List<IProcessedRule>> ApplyMatchAsync_RAS(string Context, RulesToReturn rulesToReturn)
    {
        return rulesToReturn switch
        {
            RulesToReturn.First => [await ApplyFirstMatchAsync_RAS(Context).ConfigureAwait(false)],
            RulesToReturn.All => await ApplyAllMatchAsync_RAS(Context).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(rulesToReturn), $"Invalid value: {rulesToReturn}")
        };
    }

    private async Task<IProcessedRule> ApplyFirstMatchAsync_RAS(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredRuleService.RegisteredRules.Rules == null || _registeredRuleService.RegisteredRules.Rules.Count == 0)
        {
            return GetDefaultRule();
        }
        return await RuleApplicationService.ApplyFirstAsync(_interpreter, Context, _registeredRuleService.RegisteredRules).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> ApplyAllMatchAsync_RAS(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredRuleService.RegisteredRules.Rules == null || _registeredRuleService.RegisteredRules.Rules.Count == 0)
        {
            return [GetDefaultRule()];
        }
        return await RuleApplicationService.ApplyAllAsync(_interpreter, Context, _registeredRuleService.RegisteredRules, RulesToReturn.All).ConfigureAwait(false);
    }
}
