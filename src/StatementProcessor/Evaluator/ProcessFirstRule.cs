using RightRule.Data;
using RightRule.Models;
using RightRule.RuleModels;
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
            AppliedRule appliedRule = AppliedRule.DefaultResult();

            RuleResult result = await EvaluateAsync(evaluator, rule, registeredDatasets).ConfigureAwait(false);
            switch ((result.Result.IsSuccess, result.Result.Value))
            {
                case (true, true): // Success and Rule applied
                    appliedRule = CreateTrueResult(result, rule);
                    complete = true;
                    break;

                case (true, false): // Success and Rule not applied
                    appliedRule = CreateFalseResult(result, rule);
                    continue;

                default:
                    appliedRule = CreateErrorResult(result, rule);
                    complete = true;
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
            var message = string.IsNullOrEmpty(rule.ParserMessage) ? "Rule statement not interpreted properly." : rule.ParserMessage;
            return RuleResult.Failure(message);
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
        return AppliedRule.TrueResult(rule, result);
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
