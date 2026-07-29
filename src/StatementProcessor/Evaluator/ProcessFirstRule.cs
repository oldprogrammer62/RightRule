using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;
using RightRule.System.Common;

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

            RuleResult result = await EvaluateAsync(evaluator, rule, registeredDatasets).ConfigureAwait(false);
            switch ((result.Result.IsSuccess, result.Result.Value))
            {
                case (true, true):
                    appliedRule = CreateTrueResult(result, rule);
                    complete = true;
                    break;

                case (true, false):
                    appliedRule = CreateFalseResult(result, rule);
                    continue;

                default:
                    appliedRule = CreateErrorResult(result, rule);
                    break;
            }
            if (complete)
            {
                appliedRules.Add(appliedRule);
                break;
            }
        }
        return appliedRules;
    }

    private async static Task<RuleResult> EvaluateAsync(Evaluator evaluator, ParsedRule rule, RegisteredDatasets datasets)
    {
        if (rule.Node == null)
        {
            return new RuleResult(EvaluationResult.Failure("Rule statement not interpreted properly."));
            //return new EvalResult()
            //{
            //    Result = EvalResult.ResultType.ResultError,
            //    Message = "Rule statement not interpreted properly.",
            //    StatementApplied = rule.Statement
            //};
        }

        try
        {
            RuleResult result = await evaluator.EvaluateAsync(rule, datasets).ConfigureAwait(false);
            return result;
        }
        catch
        {
            throw;
        }
    }

    private static AppliedRule CreateTrueResult(RuleResult result, ParsedRule rule)
    {
        return new AppliedRule(rule, result);
    }

    private static AppliedRule CreateFalseResult(RuleResult result, ParsedRule rule)
    {
        return AppliedRule.FalseResult(rule, result);
    }

    private static AppliedRule CreateErrorResult(RuleResult result, ParsedRule rule)
    {
        return AppliedRule.ErrorResult(rule, result);
    }
}
