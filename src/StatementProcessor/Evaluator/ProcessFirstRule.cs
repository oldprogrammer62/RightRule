using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;

namespace RightRule.StatementProcessor.Evaluator;

internal class ProcessFirstRule
{
    internal async static Task<List<AppliedRule>> EvaluateAsync(Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        List<AppliedRule> appliedRules = [];
        bool complete = false;

        foreach (var rule in rules)
        {
            AppliedRule appliedRule = AppliedRule.DefaultRule();

            EvalResult result = await EvaluateAsync(evaluator, rule, registeredDatasets).ConfigureAwait(false);
            switch (result.Result)
            {
                case EvalResult.ResultType.ResultTrue:
                    appliedRule = CreateTrueResult(result, rule);
                    complete = true;
                    break;

                case EvalResult.ResultType.ResultFalse:
                    appliedRule = CreateFalseResult(result, rule);
                    continue;

                case EvalResult.ResultType.ResultError:
                    appliedRule = CreateErrorResult(result, rule);
                    break;

                default:
                    throw new InvalidOperationException($"Unexpected result type: {result.Result}");
            }
            if (complete)
            {
                appliedRules.Add(appliedRule);
                break;
            }
        }
        return appliedRules;
    }

    private async static Task<EvalResult> EvaluateAsync(Evaluator evaluator, ParsedRule rule, RegisteredDatasets datasets)
    {
        if (rule.Node == null)
        {
            return new EvalResult()
            {
                Result = EvalResult.ResultType.ResultError,
                Message = "Rule statement not interpreted properly.",
                StatementApplied = rule.Statement
            };
        }

        try
        {
            EvalResult result = await evaluator.EvaluateAsync(rule, datasets).ConfigureAwait(false);
            return result;
        }
        catch
        {
            throw;
        }
    }

    private static AppliedRule CreateTrueResult(EvalResult result, ParsedRule rule)
    {
        return new AppliedRule(rule, result);
    }

    private static AppliedRule CreateFalseResult(EvalResult result, ParsedRule rule)
    {
        return AppliedRule.FalseResult(rule, result);
    }

    private static AppliedRule CreateErrorResult(EvalResult result, ParsedRule rule)
    {
        return AppliedRule.ErrorResult(rule, result);
    }
}
