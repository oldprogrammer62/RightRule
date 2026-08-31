using RightRule.DataRegistration;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;

namespace RightRule.StatementProcessor.Evaluator;

internal static class EvaluateFirstMatch
{
    internal async static Task<IRule> EvaluateAsync(IEvaluator evaluator, List<ParsedRule> rules, IRegisteredDatasets? registeredDatasets, IRegisteredDataObjects? registeredDataObjects)
    {
        IRule appliedRule = DefaultRule.DefaultResult();

        bool complete = false;
        bool matched = false;

        foreach (var rule in rules)
        {
            RuleEvaluationResult ruleResult = await EvaluateAsync(evaluator, rule, registeredDatasets, registeredDataObjects).ConfigureAwait(false);

            bool success = ruleResult.OperatorResult?.IsSuccess ?? false;
            bool result = ruleResult.OperatorResult?.Result ?? false;

            switch ((success, result))
            {
                case (true, true): // Successfully applied and Rule true
                    appliedRule = CreateTrueResult(ruleResult, rule);
                    complete = true;
                    matched = true;
                    break;

                case (true, false): // Successfully applied and Rule false
                    complete = false;
                    matched = false;
                    continue;

                default:
                    appliedRule = CreateErrorResult(ruleResult, rule);
                    complete = true;
                    matched = true;
                    break;
            }
            if (complete)
            {
                break;
            }
        }
        if (!matched) // appliedRule is default
        {
            appliedRule = DefaultRule.DefaultResult();
        }
        return appliedRule;
    }

    private async static Task<RuleEvaluationResult> EvaluateAsync(IEvaluator evaluator, ParsedRule rule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects)
    {
        if (rule.Node == null)
        {
            var message = string.IsNullOrEmpty(rule.Message) ? "Rule statement not interpreted properly." : rule.Message;
            return RuleEvaluationResult.Error(message);
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

    private static IRule CreateTrueResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.BuildTrueResult(rule, result);
    }

    private static IRule CreateFalseResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.BuildFalseResult(rule, result);
    }

    private static IRule CreateErrorResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return ErrorRule.ErrorResult(rule, result);
    }
}
