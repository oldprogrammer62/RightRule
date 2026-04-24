using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Interpreter;
using RightRule.System.Common;

namespace RightRule.Rules;

internal class RuleApplicationService
{
    internal static AppliedRule ApplyRules(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToApply rulesToApply, List<IOperator> Operators)
    {
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);
        return ApplyRulesToData(Context, Data, filteredRules, rulesToApply, Operators);
    }

    private static AppliedRule ApplyRulesToData(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToApply RulesToApply, List<IOperator> Operators)
    {
        var data = Data[0];
        var evaluator = new Evaluator(data.Data, Operators);
        return evaluator.Evaluate(rules, RulesToApply);
    }
}
