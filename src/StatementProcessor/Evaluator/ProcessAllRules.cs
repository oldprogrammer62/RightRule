using RightRule.DataRegistration;
using RightRule.Models;
using RightRule.RuleModels;

namespace RightRule.StatementProcessor.Evaluator;

internal static class ProcessAllRules
{
    internal async static Task<List<AppliedRule>> EvaluateAsync(IEvaluator evaluator, List<ParsedRule> rules, IRegisteredDatasets? registeredDatasets, IRegisteredDataObjects? registeredDataObjects)
    {
        List<AppliedRule> appliedRules = [];
        bool matched = false;

        foreach (var rule in rules)
        {
            AppliedRule appliedRule = AppliedRule.DefaultResult();

            RuleEvaluationResult ruleResult = await EvaluateAsync(evaluator, rule, registeredDatasets, registeredDataObjects).ConfigureAwait(false);
            switch ((ruleResult.EvaluationResult.IsSuccess, ruleResult.EvaluationResult.Result))
            {
                case (true, true): // Success and Rule applied
                    appliedRules.Add(CreateTrueResult(ruleResult, rule));
                    matched = true;
                    break;

                case (true, false): // Success and Rule not applied
                    appliedRules.Add(CreateFalseResult(ruleResult, rule));
                    matched = true;
                    break;

                default:
                    appliedRules.Add(CreateErrorResult(ruleResult, rule));
                    matched = true;
                    break;
            }
        }
        if (!matched)
        {
            appliedRules.Add(AppliedRule.DefaultResult());
        }
        return appliedRules;
    }

    private async static Task<RuleEvaluationResult> EvaluateAsync(IEvaluator evaluator, ParsedRule rule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects)
    {
        if (rule.Node == null)
        {
            var message = string.IsNullOrEmpty(rule.Message) ? "Rule statement not interpreted properly." : rule.Message;
            return RuleEvaluationResult.Failure(message, rule.DecisionData);
        }

        try
        {
            RuleEvaluationResult result = await evaluator.EvaluateAsync(rule, Datasets, DataObjects).ConfigureAwait(false);
            return result;
        }
        catch
        {
            throw;
        }
    }

    private static AppliedRule CreateTrueResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.TrueResult(rule, result);
    }

    private static AppliedRule CreateFalseResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.FalseResult(rule, result);
    }

    private static AppliedRule CreateErrorResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.ErrorResult(rule, result);
    }
}
