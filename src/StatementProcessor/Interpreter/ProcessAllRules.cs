using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;

namespace RightRule.StatementProcessor.Interpreter;

internal class ProcessAllRules
{
    internal static List<AppliedRule> EvaluateAll(Evaluator.Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        AppliedRule resultRule = AppliedRule.DefaultRule();
        List<AppliedRule> appliedRules = [];

        foreach (var rule in rules)
        {
            for (var i = 0; i < registeredDatasets.Count; i++)
            {
                var dataset = registeredDatasets[i];
                if (dataset == null)
                {
                    continue;
                }

                var result = Evaluate(evaluator, rule, registeredDatasets, dataset);
                var appliedRule = CreateAppliedRule(result, rule, dataset);
                appliedRules.Add(appliedRule);
            }
        }
        return appliedRules;
    }

    internal static List<AppliedRule> EvaluateAll(Evaluator.Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets, string datasetName)
    {
        var data = registeredDatasets.GetDataset(datasetName);
        if (data == null)
        {
            return [AppliedRule.DefaultRule($"Dataset '{datasetName}' not found.")];
        }

        AppliedRule resultRule = AppliedRule.DefaultRule();
        List<AppliedRule> appliedRules = [];

        foreach (var rule in rules)
        {
            var result = Evaluate(evaluator, rule, registeredDatasets, data);
            var appliedRule = CreateAppliedRule(result, rule, data);
            appliedRules.Add(appliedRule);
        }
        return appliedRules;
    }

    private static EvalResult Evaluate(Evaluator.Evaluator evaluator, ParsedRule rule, RegisteredDatasets datasets, RegisteredDataset dataset)
    {
        try
        {
            var result = evaluator.Evaluate(datasets, dataset, rule.Node);
            return new EvalResult(result, string.Empty);
        }
        catch (KeyNotFoundException knf)
        {
            if (evaluator.SilentErrors)
            {
                return new EvalResult()
                {
                    Result = EvalResult.ResultType.ResultError,
                    Message = knf.Message
                };
            }
            throw;
        }
        catch (Exception ex)
        {
            if (evaluator.SilentErrors)
            {
                return new EvalResult()
                {
                    Result = EvalResult.ResultType.ResultError,
                    Message = ex.Message
                };
            }
            throw;
        }
    }

    private static AppliedRule CreateAppliedRule(EvalResult result, ParsedRule rule, RegisteredDataset dataset)
    {
        return new AppliedRule(rule, dataset.Name, rule.Statement, result);
    }
}
