using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.RuleApplication;

internal class RuleApplicationService
{
    internal async static Task<List<AppliedRule>> ApplyRuleAsync(string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects,  IRegisteredRules parsedRules, Dictionary<string, Operator> Operators)
    {
        return await ApplyRuleToDataAsync(Context, Datasets, DataObjects, parsedRules, Operators).ConfigureAwait(false);
    }

    private async static Task<List<AppliedRule>> ApplyRuleToDataAsync(string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, IRegisteredRules parsedRules, Dictionary<string, Operator> Operators)
    {
        var interpreter = new Interpreter(Datasets, DataObjects, Operators);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRuleAsync(filteredRules).ConfigureAwait(false);
    }

    internal async static Task<List<AppliedRule>> ApplyRulesAsync(string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, IRegisteredRules parsedRules, RulesToReturn ReturnRules, Dictionary<string, Operator> Operators)
    {
        return await ApplyRulesToDataAsync(Context, Datasets, DataObjects, parsedRules, ReturnRules, Operators).ConfigureAwait(false);
    }

    private async static Task<List<AppliedRule>> ApplyRulesToDataAsync(string Context, IRegisteredDatasets? Datasets, IRegisteredDataObjects? Dataobjects, IRegisteredRules parsedRules, RulesToReturn ReturnRules, Dictionary<string, Operator> Operators)
    {
        var interpreter = new Interpreter(Datasets, Dataobjects, Operators);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRulesAsync(filteredRules, ReturnRules).ConfigureAwait(false);
    }
}