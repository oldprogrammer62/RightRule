using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.RuleModels;
using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Evaluator;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.RuleApplication;

internal class RuleApplicationService
{
    internal async static Task<List<AppliedRule>> ApplyRuleAsync(IEvaluator evaluator, string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, IRegisteredRules parsedRules, Dictionary<string, Operator> Operators)
    {
        return await ApplyRuleToDataAsync(evaluator, Context, Datasets, DataObjects, parsedRules, Operators).ConfigureAwait(false);
    }

    internal async static Task<List<AppliedRule>> ApplyRulesAsync(IEvaluator evaluator, string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, IRegisteredRules parsedRules, RulesToReturn ReturnRules, Dictionary<string, Operator> Operators)
    {
        return await ApplyRulesToDataAsync(evaluator, Context, Datasets, DataObjects, parsedRules, ReturnRules, Operators).ConfigureAwait(false);
    }

    private async static Task<List<AppliedRule>> ApplyRuleToDataAsync(IEvaluator evaluator, string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, IRegisteredRules parsedRules, Dictionary<string, Operator> Operators)
    {
        var interpreter = new Interpreter(evaluator, Datasets, DataObjects, Operators);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRuleAsync(filteredRules).ConfigureAwait(false);
    }

    private async static Task<List<AppliedRule>> ApplyRulesToDataAsync(IEvaluator evaluator, string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? Dataobjects, IRegisteredRules parsedRules, RulesToReturn ReturnRules, Dictionary<string, Operator> Operators)
    {
        var interpreter = new Interpreter(evaluator, Datasets, Dataobjects, Operators);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRulesAsync(filteredRules, ReturnRules).ConfigureAwait(false);
    }
}