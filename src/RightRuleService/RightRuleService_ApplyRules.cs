using RightRule.Models;
using RightRule.Models.BaseModels;
using RightRule.RuleApplication;
using RightRule.System.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public async Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyRulesAsync(Context, RulesToReturn.First).ConfigureAwait(false);
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

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyMatchAsync(Context, rulesToReturn).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
        }
        return BuildResult(rulesApplied);
    }

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn, DataRetention dataRetention)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        var rulesApplied = await ApplyMatchAsync(Context, DataName, Data, rulesToReturn, dataRetention).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
        }
        return BuildResult(rulesApplied);
    }

    private async Task<List<IProcessedRule>> ApplyMatchAsync(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return rulesToReturn switch
        {
            RulesToReturn.First => [await ApplyFirstMatchAsync_RAS(Context).ConfigureAwait(false)],
            RulesToReturn.All => await ApplyAllMatchAsync_RAS(Context).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(rulesToReturn), $"Invalid value: {rulesToReturn}")
        };
    }

    private async Task<List<IProcessedRule>> ApplyMatchAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn, DataRetention dataRetention)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        RegisterData(DataName, Data);
        var result = rulesToReturn switch
        {
            RulesToReturn.First => [await ApplyFirstMatchAsync_RAS(Context).ConfigureAwait(false)],
            RulesToReturn.All => await ApplyAllMatchAsync_RAS(Context).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(rulesToReturn), $"Invalid value: {rulesToReturn}")
        };
        if (dataRetention.Equals(DataRetention.Discard))
        {
            DeRegisterData(DataName);
        }
        return result;
    }

    // *****************************************
    // *** Calls to RULE APPLICATION SERVICE ***
    // *****************************************

    private async Task<IProcessedRule> ApplyFirstMatchAsync_RAS(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredRuleService.RegisteredRules.Rules == null || _registeredRuleService.RegisteredRules.Rules.Count == 0)
        {
            return GetDefaultRule(Context);
        }
        return await RuleApplicationService.ApplyFirstAsync(LanguageInterpreter, Context, _registeredRuleService.RegisteredRules).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> ApplyAllMatchAsync_RAS(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredRuleService.RegisteredRules.Rules == null || _registeredRuleService.RegisteredRules.Rules.Count == 0)
        {
            return [GetDefaultRule(Context)];
        }
        return await RuleApplicationService.ApplyAllAsync(LanguageInterpreter, Context, _registeredRuleService.RegisteredRules, RulesToReturn.All).ConfigureAwait(false);
    }
}
