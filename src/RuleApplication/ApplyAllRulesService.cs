using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.RuleApplication;

internal class ApplyAllRulesService : IApplyAllRulesService
{
    private readonly IInterpreter _interpreter;
    private readonly IRegisteredRules _registeredRules;

    public ApplyAllRulesService(IInterpreter Interpreter, IRegisteredRules RegisteredRules)
    {
        this._interpreter = Interpreter;
        this._registeredRules = RegisteredRules;
    }

    public async Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        return await EvaluateAllAsync(Context, EvaluationType).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> EvaluateAllAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        var filteredRules = Helpers.FilterRules(Context, _registeredRules.Rules);
        return await _interpreter.EvaluateAllAsync(filteredRules, Context, EvaluationType).ConfigureAwait(false);
    }
}
