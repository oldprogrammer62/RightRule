using RightRule.Data;
using RightRule.HelperFunctions;
using RightRule.Interpreter;
using RightRule.System.Common;

namespace RightRule.Rules;

internal class RuleApplicationService
{
    internal static AppliedRule ApplyRules(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToApply rulesToApply, List<IOperator> Operators, Dictionary<string, IOperator> OperatorNames)
    {
        return ApplyRulesToData(Context, Data, rules, rulesToApply, Operators, OperatorNames);
    }

    private static AppliedRule ApplyRulesToData(string Context, RegisteredDatasets Data, ParsedRules rules, RulesToApply RulesToApply, List<IOperator> Operators, Dictionary<string, IOperator> OperatorNames)
    {
        var evaluator = new Evaluator(Data, Operators, OperatorNames);
        var filteredRules = Helpers.FilterRules(Context, rules.RightRules);

        return evaluator.Evaluate(filteredRules, RulesToApply);
    }
}
