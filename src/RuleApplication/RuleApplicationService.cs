using RightRule.LanguageProcessor.Interpreter;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.RuleRegistration;

namespace RightRule.RuleApplication;

internal class RuleApplicationService
{
    internal async static Task<IProcessedRule> ApplyFirstAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, InewGetDataService getDataService)
    {
        return await EvaluateFirstMatchAsync(interpreter, Context, parsedRules, getDataService).ConfigureAwait(false);
    }

    internal async static Task<List<IProcessedRule>> ApplyAllAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, RulesToReturn ReturnRules, InewGetDataService getDataService)
    {
        return await EvaluateAllAsync(interpreter, Context, parsedRules, ReturnRules, getDataService).ConfigureAwait(false);
    }

    private async static Task<IProcessedRule> EvaluateFirstMatchAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, InewGetDataService getDataService)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.EvaluateFirstAsync(filteredRules, Context, getDataService).ConfigureAwait(false);
    }

    private async static Task<List<IProcessedRule>> EvaluateAllAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, RulesToReturn ReturnRules, InewGetDataService getDataService)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.EvaluateAllAsync(filteredRules, Context, ReturnRules, getDataService).ConfigureAwait(false);
    }
}
