using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DefaultRuleRegistration;

namespace RightRule.LanguageProcessor.Evaluator;

internal static class EvaluateFirstMatch
{
    internal async static Task<IProcessedRule> EvaluateAsync(IEvaluator evaluator, string Context, List<ParsedRule> rules, IDefaultRuleRegister defaultRuleRegister)
    {
        IProcessedRule appliedRule = defaultRuleRegister.GetDefaultRule(Context);

        bool complete = false;
        bool matched = false;

        foreach (var rule in rules)
        {
            EvaluationResult ruleResult = await EvaluateAsync(evaluator, rule).ConfigureAwait(false);

            bool success = ruleResult.Success;
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

    private async static Task<EvaluationResult> EvaluateAsync(IEvaluator evaluator, ParsedRule rule)
    {
        if (rule.Node == null)
        {
            var message = string.IsNullOrEmpty(rule.Message) ? "Rule statement not interpreted properly." : rule.Message;
            return EvaluationResult.ErrorResult(message);
        }

        try
        {
            EvaluationResult result = await evaluator.EvaluateAsync(rule).ConfigureAwait(false);
            return result;
        }
        catch
        {
            throw;
        }
    }

    private static IProcessedRule CreateTrueResult(EvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.TrueResult(rule, result);
    }

    private static IProcessedRule CreateFalseResult(EvaluationResult result, ParsedRule rule)
    {
        return AppliedRule.FalseResult(rule, result);
    }

    private static IProcessedRule CreateErrorResult(EvaluationResult result, ParsedRule rule)
    {
        return ErrorRule.ErrorResult(rule, result);
    }
}
