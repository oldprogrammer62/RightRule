using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.RuleApplication;

internal class RuleApplicationService
{
    internal async static Task<List<AppliedRule>> ApplyRuleAsync(string Context, RegisteredDatasets? Datasets, RegisteredDataObjects? DataObjects,  RegisteredRules parsedRules, Dictionary<string, Operator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        return await ApplyRuleToDataAsync(Context, Datasets, DataObjects, parsedRules, Operators, rightRuleConfig).ConfigureAwait(false);
    }

    private async static Task<List<AppliedRule>> ApplyRuleToDataAsync(string Context, RegisteredDatasets? Datasets, RegisteredDataObjects? DataObjects, RegisteredRules parsedRules, Dictionary<string, Operator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        var interpreter = new Interpreter(Datasets, DataObjects, Operators, rightRuleConfig);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRuleAsync(filteredRules).ConfigureAwait(false);
    }

    internal async static Task<List<AppliedRule>> ApplyRulesAsync(string Context, RegisteredDatasets? Datasets, RegisteredDataObjects? DataObjects, RegisteredRules parsedRules, RulesToReturn rulesToReturn, Dictionary<string, Operator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        return await ApplyRulesToDataAsync(Context, Datasets, DataObjects, parsedRules, rulesToReturn, Operators, rightRuleConfig).ConfigureAwait(false);
    }

    private async static Task<List<AppliedRule>> ApplyRulesToDataAsync(string Context, RegisteredDatasets? Datasets, RegisteredDataObjects? Dataobjects, RegisteredRules parsedRules, RulesToReturn rulesToReturn, Dictionary<string, Operator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        var interpreter = new Interpreter(Datasets, Dataobjects, Operators, rightRuleConfig);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRulesAsync(filteredRules, rulesToReturn).ConfigureAwait(false);
    }
}