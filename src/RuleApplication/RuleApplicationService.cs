using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;

namespace RightRule.RuleApplication;

internal class RuleApplicationService : IRuleApplicationService
{
    private readonly IRuleService _ruleService;
    private readonly IApplyFirstRuleService _applyFirstRuleService;
    private readonly IApplyAnyRulesService _applyAnyRulesService;
    private readonly IApplyAllRulesService _applyAllRulesService;
    private readonly IDefaultRuleRegister _defaultRuleRegister;

    public RuleApplicationService(IRuleService ruleService, IApplyFirstRuleService applyFirstRuleService, IApplyAnyRulesService applyAnyRulesService, IApplyAllRulesService applyAllRulesService, IDefaultRuleRegister defaultRuleRegister)
    {
        this._ruleService = ruleService;
        this._applyFirstRuleService = applyFirstRuleService;
        this._applyAnyRulesService = applyAnyRulesService;
        this._applyAllRulesService = applyAllRulesService;
        this._defaultRuleRegister = defaultRuleRegister;
    }

    public async Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_ruleService.RegisteredRules.Rules == null || _ruleService.RegisteredRules.Rules.Count == 0)
        {
            return [GetDefaultRule(Context)];
        }

        return EvaluationType switch
        {
            EvaluationTypeEnum.FIRST => [await ApplyFirstMatchAsync_RAS(Context).ConfigureAwait(false)],
            EvaluationTypeEnum.ANY => await ApplyAnyMatchAsync_RAS(Context, EvaluationType).ConfigureAwait(false),
            EvaluationTypeEnum.ALL => await ApplyAllMatchAsync_RAS(Context, EvaluationType).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(EvaluationType), $"Invalid value: {EvaluationType}")
        };
    }

    private async Task<IProcessedRule> ApplyFirstMatchAsync_RAS(string Context)
    {
        return await _applyFirstRuleService.ApplyRuleAsync(Context);
    }

    private async Task<List<IProcessedRule>> ApplyAnyMatchAsync_RAS(string Context, EvaluationTypeEnum EvaluationType)
    {
        return await _applyAnyRulesService.ApplyRulesAsync(Context, EvaluationType);
    }

    private async Task<List<IProcessedRule>> ApplyAllMatchAsync_RAS(string Context, EvaluationTypeEnum EvaluationType)
    {
        return await _applyAllRulesService.ApplyRulesAsync(Context, EvaluationType);
    }

    private DefaultRule GetDefaultRule(string Context)
    {
        return _defaultRuleRegister.GetDefaultRule(Context);
    }
}
