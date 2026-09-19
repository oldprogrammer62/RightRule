using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RuleApplication;

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

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyMatchAsync(Context, rulesToReturn).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
        }
        return BuildResult(rulesApplied);
    }

    public async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, EvaluationTypeEnum rulesToReturn, DataRetentionEnum dataRetention)
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

    private async Task<List<IProcessedRule>> ApplyMatchAsync(string Context, EvaluationTypeEnum rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        return rulesToReturn switch
        {
            EvaluationTypeEnum.FIRST => [await ApplyFirstMatchAsync_RAS(Context).ConfigureAwait(false)],
            EvaluationTypeEnum.ANY => await ApplyAllMatchAsync_RAS(Context).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(rulesToReturn), $"Invalid value: {rulesToReturn}")
        };
    }

    private async Task<List<IProcessedRule>> ApplyMatchAsync(string Context, string DataName, Dictionary<string, object> Data, EvaluationTypeEnum rulesToReturn, DataRetentionEnum dataRetention)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        RegisterData(DataName, Data);
        var result = rulesToReturn switch
        {
            EvaluationTypeEnum.FIRST => [await ApplyFirstMatchAsync_RAS(Context).ConfigureAwait(false)],
            EvaluationTypeEnum.ANY => await ApplyAllMatchAsync_RAS(Context).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(rulesToReturn), $"Invalid value: {rulesToReturn}")
        };
        if (dataRetention.Equals(DataRetentionEnum.DISCARD))
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

        if (RuleService.RegisteredRules.Rules == null || RuleService.RegisteredRules.Rules.Count == 0)
        {
            return GetDefaultRule(Context);
        }
        return await RuleApplicationService.ApplyFirstAsync(Interpreter, Context, RuleService.RegisteredRules).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> ApplyAllMatchAsync_RAS(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (RuleService.RegisteredRules.Rules == null || RuleService.RegisteredRules.Rules.Count == 0)
        {
            return [GetDefaultRule(Context)];
        }
        return await RuleApplicationService.ApplyAllAsync(Interpreter, Context, RuleService.RegisteredRules, EvaluationTypeEnum.ANY).ConfigureAwait(false);
    }
}
