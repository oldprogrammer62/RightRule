using RightRule.DataRegistration;
using RightRule.HelperFunctions;
using RightRule.RuleModels;
using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Evaluator;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.RuleApplication;

internal class RuleApplicationService
{
    internal async static Task<List<AppliedRule>> ApplyRuleAsync(IInterpreter interpreter, string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, IRegisteredRules parsedRules, Dictionary<string, Operator> Operators)
    {
        return await ApplyRuleToDataAsync(interpreter, Context, Datasets, DataObjects, parsedRules, Operators).ConfigureAwait(false);
    }

    internal async static Task<List<AppliedRule>> ApplyRulesAsync(IInterpreter interpreter, string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, IRegisteredRules parsedRules, RulesToReturn ReturnRules, Dictionary<string, Operator> Operators)
    {
        return await ApplyRulesToDataAsync(interpreter, Context, Datasets, DataObjects, parsedRules, ReturnRules, Operators).ConfigureAwait(false);
    }

    private async static Task<List<AppliedRule>> ApplyRuleToDataAsync(IInterpreter interpreter, string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, IRegisteredRules parsedRules, Dictionary<string, Operator> Operators)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);
        return await interpreter.ProcessRuleAsync(filteredRules).ConfigureAwait(false);
    }

    private async static Task<List<AppliedRule>> ApplyRulesToDataAsync(IInterpreter interpreter, string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? Dataobjects, IRegisteredRules parsedRules, RulesToReturn ReturnRules, Dictionary<string, Operator> Operators)
    {
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRulesAsync(filteredRules, ReturnRules).ConfigureAwait(false);
    }
}