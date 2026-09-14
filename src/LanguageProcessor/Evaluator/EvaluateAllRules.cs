using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;

namespace RightRule.LanguageProcessor.Evaluator;

internal static class EvaluateAllRules
{
    internal async static Task<List<IProcessedRule>> EvaluateAsync(IEvaluator evaluator, string Context, List<ParsedRule> rules, IRegisteredDatasets? registeredDatasets, IRegisteredDataObjects? registeredDataObjects, IDefaultRuleRegister defaultRuleRegister, InewGetDataService getDataService)
    {
        List<IProcessedRule> appliedRules = new List<IProcessedRule>();
        bool matched = false;

        foreach (var rule in rules)
        {
            EvaluationResult ruleResult = await EvaluateAsync(evaluator, rule, registeredDatasets, registeredDataObjects, getDataService).ConfigureAwait(false);

            bool success = ruleResult.Success;
            bool result = ruleResult.Result;

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
            appliedRules.Add(defaultRuleRegister.GetDefaultRule(Context));
        }
        return appliedRules;
    }

    private async static Task<EvaluationResult> EvaluateAsync(IEvaluator evaluator, ParsedRule rule, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, InewGetDataService getDataService)
    {
        if (rule.Node == null)
        {
            var message = string.IsNullOrEmpty(rule.Message) ? "Rule statement not interpreted properly." : rule.Message;
            return EvaluationResult.ErrorResult(message);
        }

        try
        {
            EvaluationResult result = await evaluator.EvaluateAsync(rule, Datasets, DataObjects, getDataService).ConfigureAwait(false);
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
