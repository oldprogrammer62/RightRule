using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.System.Common;

namespace RightRule.Rules;

internal class RuleApplicationService
{
    internal static AppliedRule ApplyRule(string Context, RegisteredDatasets Data, ParsedRules rules, Dictionary<string, IOperator> Operators)
    {
        return ApplyRuleToData(Context, Data, rules, Operators);
    }

    internal static List<AppliedRule> ApplyRules(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToApply rulesToApply, Dictionary<string, IOperator> Operators)
    {
        return ApplyRulesToData(Context, Data, rules, rulesToApply, Operators);
    }

    private static List<AppliedRule> ApplyRulesToData(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToApply RulesToApply, Dictionary<string, IOperator> Operators)
    {
        var interpreter = new Interpreter.Interpreter(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return interpreter.ProcessRules(filteredRules, RulesToApply);
    }

    private static AppliedRule ApplyRuleToData(string Context, RegisteredDatasets Data, ParsedRules rules, Dictionary<string, IOperator> Operators)
    {
        var interpreter = new Interpreter.Interpreter(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return interpreter.ProcessRule(filteredRules);
    }
}