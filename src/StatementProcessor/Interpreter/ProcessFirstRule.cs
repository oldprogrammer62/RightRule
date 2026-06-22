using RightRule.Data;
using RightRule.Rules;

namespace RightRule.StatementProcessor.Interpreter;

internal class ProcessFirstRule
{
    internal static AppliedRule EvaluateFirst(Evaluator.Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        bool result = false;

        AppliedRule appliedRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            for (var i = 0; i < registeredDatasets.Count; i++)
            {
                var data = registeredDatasets[i];
                if (data == null)
                {
                    continue;
                }

                result = Evaluate(evaluator, rule, data);
                if (result)
                {
                    appliedRule = CreateAppliedRule(result, rule, data);
                    break;
                }
            }
            if (result)
            {
                break;
            }
        }
        return appliedRule;
    }

    internal static AppliedRule EvaluateFirst(Evaluator.Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets, string datasetName)
    {
        bool result = false;

        var data = registeredDatasets.GetDataset(datasetName);
        if (data == null)
        {
            return AppliedRule.DefaultRule($"Dataset '{datasetName}' not found.");
        }

        AppliedRule appliedRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            result = Evaluate(evaluator, rule, data);
            appliedRule = CreateAppliedRule(result, rule, data);

            if (result)
            {
                break;
            }
        }
        return appliedRule;
    }

    private static bool Evaluate(Evaluator.Evaluator evaluator, ParsedRule rule, RegisteredDataset data)
    {
        return evaluator.Evaluate(data, rule.Node, rule.Arguments);
    }

    private static AppliedRule CreateAppliedRule(bool result, ParsedRule rule, RegisteredDataset data)
    {
        var statement = Helper.ResolveStatement(rule.Statement, rule.Arguments);
        return new AppliedRule(rule, data.Name, statement, result);
    }
}
