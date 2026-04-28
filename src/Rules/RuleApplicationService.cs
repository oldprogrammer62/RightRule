using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Interpreter;
using RightRule.System.Common;

namespace RightRule.Rules;

internal class RuleApplicationService
{
    internal static AppliedRule ApplyRules(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToApply rulesToApply, Dictionary<string, IOperator> Operators)
    {
        return ApplyRulesToData(Context, Data, rules, rulesToApply, Operators);
    }

    private static AppliedRule ApplyRulesToData(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToApply RulesToApply, Dictionary<string, IOperator> Operators)
    {
        var evaluator = new Evaluator(Data, Operators);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return evaluator.Evaluate(filteredRules, RulesToApply);
    }
}
