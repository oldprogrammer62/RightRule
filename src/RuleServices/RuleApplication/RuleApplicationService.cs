using RightRule.Exceptions;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.Rules;

namespace RightRule.RuleServices.RuleApplication;

internal sealed class RuleApplicationService : IRuleApplicationService
{
    private readonly IRegisteredRules _registeredRules;
    private readonly IApplyFirstRuleService _applyFirstRuleService;
    private readonly IApplyAnyRulesService _applyAnyRulesService;
    private readonly IApplyAllRulesService _applyAllRulesService;

    public RuleApplicationService(IRegisteredRules registeredRules, IApplyFirstRuleService applyFirstRuleService, IApplyAnyRulesService applyAnyRulesService, IApplyAllRulesService applyAllRulesService)
    {
        this._registeredRules = registeredRules;
        this._applyFirstRuleService = applyFirstRuleService;
        this._applyAnyRulesService = applyAnyRulesService;
        this._applyAllRulesService = applyAllRulesService;
    }

    public async Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        if (_registeredRules.Rules == null || _registeredRules.Rules.Count == 0)
        {
            // Throw exception to indicate No Rules Registered
            throw new NoRulesRegisteredException();
        }

        return EvaluationType switch
        {
            EvaluationTypeEnum.FIRST => [await ApplyFirstMatchAsync(Context).ConfigureAwait(false)],
            EvaluationTypeEnum.ANY => await ApplyAnyMatchAsync(Context, EvaluationType).ConfigureAwait(false),
            EvaluationTypeEnum.ALL => await ApplyAllMatchAsync(Context, EvaluationType).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(EvaluationType), $"Invalid value: {EvaluationType}")
        };
    }

    private async Task<IProcessedRule> ApplyFirstMatchAsync(string Context)
    {
        return await _applyFirstRuleService.ApplyRuleAsync(Context);
    }

    private async Task<List<IProcessedRule>> ApplyAnyMatchAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        return await _applyAnyRulesService.ApplyRulesAsync(Context, EvaluationType);
    }

    private async Task<List<IProcessedRule>> ApplyAllMatchAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        return await _applyAllRulesService.ApplyRulesAsync(Context, EvaluationType);
    }
}
