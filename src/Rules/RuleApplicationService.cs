using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Models;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.Rules;

internal class RuleApplicationService
{
    internal async static Task<List<AppliedRule>> ApplyRule(string Context, RegisteredDatasets Datasets, RegisteredRules parsedRules, Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        return await ApplyRuleToData(Context, Datasets, parsedRules, Operators, rightRuleConfig);
    }

    private async static Task<List<AppliedRule>> ApplyRuleToData(string Context, RegisteredDatasets Datasets, RegisteredRules parsedRules, Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        var interpreter = new Interpreter(Datasets, Operators, rightRuleConfig);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRule(filteredRules);
    }

    internal async static Task<List<AppliedRule>> ApplyRules(string Context, RegisteredDatasets Datasets, RegisteredRules parsedRules, RulesToReturn rulesToReturn, Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        return await ApplyRulesToData(Context, Datasets, parsedRules, rulesToReturn, Operators, rightRuleConfig);
    }

    private async static Task<List<AppliedRule>> ApplyRulesToData(string Context, RegisteredDatasets Datasets, RegisteredRules parsedRules, RulesToReturn rulesToReturn, Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        var interpreter = new Interpreter(Datasets, Operators, rightRuleConfig);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return await interpreter.ProcessRules(filteredRules, rulesToReturn);
    }
}