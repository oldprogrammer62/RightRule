using RightRule.HelperFunctions;
using RightRule.RuleModels.BaseModels;
using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

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
        return await interpreter.EvaluateFirstAsync(filteredRules).ConfigureAwait(false);
    }

    private async static Task<List<IProcessedRule>> EvaluateAllAsync(IInterpreter interpreter, string Context, IRegisteredRules parsedRules, RulesToReturn ReturnRules)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.EvaluateAllAsync(filteredRules, ReturnRules).ConfigureAwait(false);
    }
}
