using RightRule.DataRegistration;
using RightRule.Models.RuleEvaluation;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.RuleRegistration.DefaultRuleRegistration;

namespace RightRule.StatementProcessor.Evaluator;

internal static class EvaluateFirstMatch
{
    internal async static Task<IProcessedRule> EvaluateAsync(IEvaluator evaluator, string Context, List<ParsedRule> rules, IRegisteredDatasets? registeredDatasets, IRegisteredDataObjects? registeredDataObjects, IDefaultRuleRegister defaultRuleRegister)
    {
        IProcessedRule appliedRule = defaultRuleRegister.GetDefaultRule(Context);

        bool complete = false;
        bool matched = false;

        foreach (var rule in rules)
        {
            RuleEvaluationResult ruleResult = await EvaluateAsync(evaluator, rule, registeredDatasets, registeredDataObjects).ConfigureAwait(false);

            bool success = ruleResult.IsSuccess;
            bool result = ruleResult.Result;

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
            appliedRule = defaultRuleRegister.GetDefaultRule(Context);
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

    private static IProcessedRule CreateTrueResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.BuildTrueResult(rule, result);
    }

    private static IProcessedRule CreateFalseResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.BuildFalseResult(rule, result);
    }

    private static IProcessedRule CreateErrorResult(RuleEvaluationResult result, ParsedRule rule)
    {
        return ErrorRule.ErrorResult(rule, result);
    }
}
