using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.Rules;

namespace RightRule.RuleServices.RuleApplication;

internal class ApplyAnyRulesService : IApplyAnyRulesService
{
    private readonly IInterpreter _interpreter;
    private readonly IRegisteredRules _registeredRules;

    public ApplyAnyRulesService(IInterpreter Interpreter, IRegisteredRules RegisteredRules)
    {
        this._interpreter = Interpreter;
        this._registeredRules = RegisteredRules;
    }

    public async Task<List<IProcessedRule>> ApplyRulesAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        return await EvaluateAnyAsync(Context, EvaluationType).ConfigureAwait(false);
    }

    private async Task<List<IProcessedRule>> EvaluateAnyAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        var filteredRules = Helpers.FilterRules(Context, _registeredRules.Rules);
        return await _interpreter.EvaluateAnyAsync(filteredRules, Context, EvaluationType).ConfigureAwait(false);
    }
}
