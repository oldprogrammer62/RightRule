using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Models;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.Rules;

internal class RuleApplicationService
{
    internal static List<AppliedRule> ApplyRule(string Context, RegisteredDatasets Datasets, RegisteredRules parsedRules, Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        return ApplyRuleToData(Context, Datasets, parsedRules, Operators, rightRuleConfig);
    }

    private static List<AppliedRule> ApplyRuleToData(string Context, RegisteredDatasets Datasets, RegisteredRules parsedRules, Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        var interpreter = new Interpreter(Datasets, Operators, rightRuleConfig);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return interpreter.ProcessRule(filteredRules);
    }

    internal static List<AppliedRule> ApplyRules(string Context, RegisteredDatasets Datasets, RegisteredRules parsedRules, RulesToReturn rulesToReturn, Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        return ApplyRulesToData(Context, Datasets, parsedRules, rulesToReturn, Operators, rightRuleConfig);
    }

    private static List<AppliedRule> ApplyRulesToData(string Context, RegisteredDatasets Datasets, RegisteredRules parsedRules, RulesToReturn rulesToReturn, Dictionary<string, IOperator> Operators, RightRuleConfiguration rightRuleConfig)
    {
        var interpreter = new Interpreter(Datasets, Operators, rightRuleConfig);
        var filteredRules = Helpers.FilterRules(Context, parsedRules.Rules);

        return interpreter.ProcessRules(filteredRules, rulesToReturn);
    }
}