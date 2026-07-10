using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;

namespace RightRule.StatementProcessor.Evaluator;

internal class ProcessAllRules
{
    internal static List<AppliedRule> EvaluateAll(Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        AppliedRule resultRule = AppliedRule.DefaultRule();
        List<AppliedRule> appliedRules = [];

        foreach (var rule in rules)
        {
            var result = Evaluate(evaluator, rule, registeredDatasets);
            var appliedRule = CreateAppliedRule(result, rule);
            appliedRules.Add(appliedRule);
        }
        return appliedRules;
    }

    private static EvalResult Evaluate(Evaluator evaluator, ParsedRule rule, RegisteredDatasets datasets)
    {
        try
        {
            var result = evaluator.Evaluate(datasets, rule.Node);
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

    private static AppliedRule CreateAppliedRule(EvalResult result, ParsedRule rule)
    {
        return new AppliedRule(rule, rule.Statement, result);
    }
}
