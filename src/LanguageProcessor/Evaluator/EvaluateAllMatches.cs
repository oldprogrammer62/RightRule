using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DefaultRuleRegistration;

namespace RightRule.LanguageProcessor.Evaluator;

internal static class EvaluateAllMatches
{
    internal async static Task<List<IProcessedRule>> EvaluateAsync(IEvaluator Evaluator, string Context, List<ParsedRule> Rules, IDefaultRuleRegister DefaultRuleRegister)
    {
        List<IProcessedRule> appliedRules = new List<IProcessedRule>();

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
                    appliedRules.Add(CreateTrueResult(ruleResult, rule));
                    complete = false;
                    matched = true;
                    continue;

                case (true, false): // Successfully applied and Rule false
                    appliedRules = new List<IProcessedRule>();
                    appliedRules.Add(DefaultRuleRegister.GetDefaultRule(Context));
                    complete = true;
                    matched = true;
                    break;

                default:
                    appliedRules.Add(CreateErrorResult(ruleResult, rule));
                    complete = true;
                    matched = true;
                    break;
            }
            if (complete)
            {
                break;
            }
        }
        if (!matched) // appliedRules is empty
        {
            appliedRules.Add(DefaultRuleRegister.GetDefaultRule(Context));
        }
        return appliedRules;
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
