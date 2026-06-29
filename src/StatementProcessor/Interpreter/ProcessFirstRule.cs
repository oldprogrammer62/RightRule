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
                var data = registeredDatasets[i];
                if (data == null)
                {
                    continue;
                }

                result = Evaluate(evaluator, rule, data);
                if (result.Result.Equals(EvalResult.ResultType.ResultTrue))
                {
                    appliedRule = CreateAppliedRule(result, rule, data);
                    break;
                }
                if (result.Result.Equals(EvalResult.ResultType.ResultError))
                {
                    appliedRule = CreateAppliedRule(result, rule, data);
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
        var data = registeredDatasets.GetDataset(datasetName);
        if (data == null)
        {
            return AppliedRule.DefaultRule($"Dataset '{datasetName}' not found.");
        }

        AppliedRule appliedRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            var result = Evaluate(evaluator, rule, data);
            appliedRule = CreateAppliedRule(result, rule, data);

            if (result.Result.Equals(EvalResult.ResultType.ResultTrue))
            {
                break;
            }
            if (result.Result.Equals(EvalResult.ResultType.ResultError))
            {
                appliedRule = CreateAppliedRule(result, rule, data);
                break;
            }
        }
        return appliedRule;
    }

    private static EvalResult Evaluate(Evaluator.Evaluator evaluator, ParsedRule rule, RegisteredDataset data)
    {
        try
        {
            var result = evaluator.Evaluate(data, rule.Node, rule.Arguments);
            return new EvalResult(result, string.Empty);
        }
        catch (Exception ex)
        {
            return new EvalResult()
            {
                Result = EvalResult.ResultType.ResultError,
                Message = ex.Message
            };
        }
    }

    private static AppliedRule CreateAppliedRule(EvalResult result, ParsedRule rule, RegisteredDataset data)
    {
        var statement = Helper.ResolveStatement(rule.Statement, rule.Arguments);
        return new AppliedRule(rule, data.Name, statement, result);
    }
}
