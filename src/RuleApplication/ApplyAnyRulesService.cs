using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.RuleApplication;

internal class ApplyAnyRulesService : IApplyAnyRulesService
{
    private readonly IInterpreter _interpreter;
    private readonly IRegisteredRules _registeredRules;

    public ApplyAnyRulesService(IInterpreter interpreter, IRegisteredRules registeredRules)
    {
        this._interpreter = interpreter;
        this._registeredRules = registeredRules;
    }

    public async Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType)
    {
        return await EvaluateAnyAsync(Context, evaluationType).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> EvaluateAnyAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        var filteredRules = Helpers.FilterRules(Context, _registeredRules.Rules);
        return await _interpreter.EvaluateAnyAsync(filteredRules, Context, EvaluationType).ConfigureAwait(false);
    }
}
