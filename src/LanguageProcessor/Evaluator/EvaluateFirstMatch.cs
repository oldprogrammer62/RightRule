using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.LanguageProcessor.Evaluator;

internal static class EvaluateFirstMatch
{
    internal async static Task<IProcessedRule> EvaluateAsync(IEvaluator Evaluator, string Context, List<ParsedRule> Rules, IDefaultRuleRegister DefaultRuleRegister)
    {
        IProcessedRule appliedRule = DefaultRuleRegister.GetDefaultRule(Context);

        bool complete = false;
        bool matched = false;

        foreach (var rule in Rules)
        {
            EvaluationResult ruleResult = await EvaluateAsync(Evaluator, rule).ConfigureAwait(false);

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
            appliedRule = DefaultRuleRegister.GetDefaultRule(Context);
        }
        return appliedRule;
    }

    private async static Task<EvaluationResult> EvaluateAsync(IEvaluator Evaluator, ParsedRule Rule)
    {
        if (Rule.Node == null)
        {
            var message = string.IsNullOrEmpty(Rule.Message) ? "Rule statement not interpreted properly." : Rule.Message;
            return EvaluationResult.ErrorResult(message);
        }

        try
        {
            EvaluationResult result = await Evaluator.EvaluateAsync(Rule).ConfigureAwait(false);
            return result;
        }
        catch
        {
            throw;
        }
    }

    private static IProcessedRule CreateTrueResult(EvaluationResult Result, ParsedRule Rule)
    {
        return AppliedRule.TrueResult(Rule, Result);
    }

    private static IProcessedRule CreateFalseResult(EvaluationResult Result, ParsedRule Rule)
    {
        return AppliedRule.FalseResult(Rule, Result);
    }

    private static IProcessedRule CreateErrorResult(EvaluationResult Result, ParsedRule Rule)
    {
        return ErrorRule.ErrorResult(Rule, Result);
    }
}
