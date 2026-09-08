using RightRule.HelperFunctions;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.RegistrationServices.Rules;
using RightRule.RuleModels.BaseModels;

namespace RightRule.RuleApplication;

internal class RuleApplicationService
{
    internal async static Task<IProcessedRule> ApplyFirstAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules)
    {
        return await EvaluateFirstMatchAsync(interpreter, Context, parsedRules).ConfigureAwait(false);
    }

    internal async static Task<List<IProcessedRule>> ApplyAllAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, RulesToReturn ReturnRules)
    {
        return await EvaluateAllAsync(interpreter, Context, parsedRules, ReturnRules).ConfigureAwait(false);
    }

    private async static Task<IProcessedRule> EvaluateFirstMatchAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.EvaluateFirstAsync(filteredRules, Context).ConfigureAwait(false);
    }

    private async static Task<List<IProcessedRule>> EvaluateAllAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, RulesToReturn ReturnRules)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.EvaluateAllAsync(filteredRules, Context, ReturnRules).ConfigureAwait(false);
    }
}
