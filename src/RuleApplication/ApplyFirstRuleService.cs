using RightRule.LanguageProcessor.Interpreter;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.RuleApplication;

internal class ApplyFirstRuleService : IApplyFirstRuleService
{
    private readonly IInterpreter _interpreter;
    private readonly IRegisteredRules _registeredRules;

    public ApplyFirstRuleService(IInterpreter Interpreter, IRegisteredRules RegisteredRules)
    {
        this._interpreter = Interpreter;
        this._registeredRules = RegisteredRules;
    }

    public async Task<IProcessedRule> ApplyRuleAsync(string Context)
    {
        return await EvaluateFirstMatchAsync(Context).ConfigureAwait(false);
    }

    private async Task<IProcessedRule> EvaluateFirstMatchAsync(string Context)
    {
        var filteredRules = Helpers.FilterRules(Context, _registeredRules.Rules);
        return await _interpreter.EvaluateFirstAsync(filteredRules, Context).ConfigureAwait(false);
    }
}
