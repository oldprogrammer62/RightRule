using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.RuleApplication;

internal class ApplyAllRulesService : IApplyAllRulesService
{
    private readonly IInterpreter _interpreter;
    private readonly IRegisteredRules _registeredRules;

    public ApplyAllRulesService(IInterpreter interpreter, IRegisteredRules registeredRules)
    {
        this._interpreter = interpreter;
        this._registeredRules = registeredRules;
    }

    public async Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType)
    {
        return await EvaluateAllAsync(Context, evaluationType).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> EvaluateAllAsync(string Context, EvaluationTypeEnum evaluationType)
    {
        var filteredRules = Helpers.FilterRules(Context, _registeredRules.Rules);
        return await _interpreter.EvaluateAllAsync(filteredRules, Context, evaluationType).ConfigureAwait(false);
    }
}
