using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration.Rules;

namespace RightRule.RuleApplication;

internal class RuleApplicationService
{
    internal async static Task<IProcessedRule> ApplyFirstAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules)
    {
        return await EvaluateFirstMatchAsync(interpreter, Context, parsedRules).ConfigureAwait(false);
    }

    internal async static Task<List<IProcessedRule>> ApplyAnyAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, EvaluationTypeEnum EvaluationType)
    {
        return await EvaluateAnyAsync(interpreter, Context, parsedRules, EvaluationType).ConfigureAwait(false);
    }

    internal async static Task<List<IProcessedRule>> ApplyAllAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, EvaluationTypeEnum EvaluationType)
    {
        return await EvaluateAllAsync(interpreter, Context, parsedRules, EvaluationType).ConfigureAwait(false);
    }

    private async static Task<IProcessedRule> EvaluateFirstMatchAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.EvaluateFirstAsync(filteredRules, Context).ConfigureAwait(false);
    }

    private async static Task<List<IProcessedRule>> EvaluateAnyAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, EvaluationTypeEnum EvaluationType)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.EvaluateAnyAsync(filteredRules, Context, EvaluationType).ConfigureAwait(false);
    }

    private async static Task<List<IProcessedRule>> EvaluateAllAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, EvaluationTypeEnum EvaluationType)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.EvaluateAllAsync(filteredRules, Context, EvaluationType).ConfigureAwait(false);
    }
}
