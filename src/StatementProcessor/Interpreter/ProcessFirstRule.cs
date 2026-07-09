using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;

namespace RightRule.StatementProcessor.Interpreter;

internal class ProcessFirstRule
{
    internal static AppliedRule EvaluateFirst(Evaluator.Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        EvalResult result = new();

        AppliedRule appliedRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            for (var i = 0; i < registeredDatasets.Count; i++)
            {
                var dataset = registeredDatasets[i];
                if (dataset == null)
                {
                    continue;
                }
                if (dataset.StaticData) // Do not process rules against static datasets
                {
                    // ISSUE
                    // What happens if NO dataset is found that is not static?
                    continue;
                }

                result = Evaluate(evaluator, rule, registeredDatasets, dataset);
                if (result.Result.Equals(EvalResult.ResultType.ResultTrue))
                {
                    appliedRule = CreateAppliedRule(result, rule, dataset);
                    break;
                }
                if (result.Result.Equals(EvalResult.ResultType.ResultError))
                {
                    appliedRule = CreateAppliedRule(result, rule, dataset);
                    break;
                }
            }
            if (result.Result.Equals(EvalResult.ResultType.ResultTrue))
            {
                break;
            }
        }
        return appliedRule;
    }

    internal static AppliedRule EvaluateFirst(Evaluator.Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets, string datasetName)
    {
        var dataset = registeredDatasets.GetDataset(datasetName);
        if (dataset == null)
        {
            return AppliedRule.DefaultRule($"Dataset '{datasetName}' not found.");
        }

        AppliedRule appliedRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            var result = Evaluate(evaluator, rule, registeredDatasets, dataset);
            appliedRule = CreateAppliedRule(result, rule, dataset);

            if (result.Result.Equals(EvalResult.ResultType.ResultTrue))
            {
                break;
            }
            if (result.Result.Equals(EvalResult.ResultType.ResultError))
            {
                appliedRule = CreateAppliedRule(result, rule, dataset);
                break;
            }
        }
        return appliedRule;
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
