using RightRule.DataRegistration;
using RightRule.Models;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;

namespace RightRule.StatementProcessor.Evaluator;

internal static class EvaluateAllRules
{
    internal async static Task<List<IRule>> EvaluateAsync(IEvaluator evaluator, List<ParsedRule> rules, IRegisteredDatasets? registeredDatasets, IRegisteredDataObjects? registeredDataObjects)
    {
        List<IRule> appliedRules = [];
        bool matched = false;

        foreach (var rule in rules)
        {
            RuleEvaluationResult ruleResult = await EvaluateAsync(evaluator, rule, registeredDatasets, registeredDataObjects).ConfigureAwait(false);

            bool success = ruleResult.OperatorResult?.IsSuccess ?? false;
            bool result = ruleResult.OperatorResult?.Result ?? false;

            switch ((success, result))
            {
                case (true, true): // Successfully applied and Rule true
                    appliedRules.Add(CreateTrueResult(ruleResult, rule));
                    matched = true;
                    break;

                case (true, false): // Successfully applied and Rule false
                    appliedRules.Add(CreateFalseResult(ruleResult, rule));
                    matched = true;
                    break;

                default:
                    appliedRules.Add(CreateErrorResult(ruleResult, rule));
                    matched = true;
                    break;
            }
        }
        if (!matched) // appliedRules is empty
        {
            appliedRules.Add(DefaultRule.DefaultResult());
        }
        return appliedRules;
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
