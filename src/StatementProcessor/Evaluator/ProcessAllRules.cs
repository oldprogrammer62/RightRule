using RightRule.Data;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Evaluator;

internal class ProcessAllRules
{
    internal async static Task<List<AppliedRule>> EvaluateAsync(Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        List<AppliedRule> appliedRules = [];

        foreach (var rule in rules)
        {
            AppliedRule appliedRule = AppliedRule.DefaultResult();

            RuleResult result = await EvaluateAsync(evaluator, rule, registeredDatasets).ConfigureAwait(false);
            switch ((result.Result.IsSuccess, result.Result.Value))
            {
                case (true, true): // Success and Rule applied
                    appliedRule = CreateTrueResult(result, rule);
                    break;

                case (true, false): // Success and Rule not applied
                    appliedRule = CreateFalseResult(result, rule);
                    break;

                default:
                    appliedRule = CreateErrorResult(result, rule);
                    break;
            }
            appliedRules.Add(appliedRule);
        }
        return appliedRules;
    }

    private async static Task<RuleResult> EvaluateAsync(Evaluator evaluator, ParsedRule rule, RegisteredDatasets datasets)
    {
        if (rule.Node == null)
        {
            return new RuleResult(EvaluationResult.Failure("Rule statement not interpreted properly."));
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
