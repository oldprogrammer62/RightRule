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
            EvalResult result = Evaluate(evaluator, rule, registeredDatasets);
            AppliedRule appliedRule = CreateAppliedRule(result, rule);
            appliedRules.Add(appliedRule);
        }
        return appliedRules;
    }

    private static EvalResult Evaluate(Evaluator evaluator, ParsedRule rule, RegisteredDatasets datasets)
    {
        try
        {
            EvalResult result = evaluator.Evaluate(rule.Statement, datasets, rule.Node);
            return result;
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
        return new AppliedRule(rule, result);
    }
}
