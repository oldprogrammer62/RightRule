using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;

namespace RightRule.StatementProcessor.Evaluator;

internal class ProcessAllRules
{
    internal static List<AppliedRule> EvaluateAll(Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        List<AppliedRule> appliedRules = [];

        foreach (var rule in rules)
        {
            AppliedRule appliedRule = AppliedRule.DefaultRule();

            EvalResult result = Evaluate(evaluator, rule, registeredDatasets);
            switch (result.Result)
            {
                case EvalResult.ResultType.ResultTrue:
                    appliedRule = CreateTrueResult(result, rule);
                    break;

                case EvalResult.ResultType.ResultFalse:
                    appliedRule = CreateFalseResult(result, rule);
                    break;

                case EvalResult.ResultType.ResultError:
                    appliedRule = CreateErrorResult(result, rule);
                    break;

                default:
                    break;
            }
            appliedRules.Add(appliedRule);
        }
        return appliedRules;
    }

    private static EvalResult Evaluate(Evaluator evaluator, ParsedRule rule, RegisteredDatasets datasets)
    {
        if (rule.Node == null)
        {
            return new EvalResult()
            {
                Result = EvalResult.ResultType.ResultError,
                Message = "Rule statement not interpreted properly.",
                StatementApplied = rule.Statement
            };
        }

        try
        {
            EvalResult result = evaluator.Evaluate(rule.Statement, datasets, rule.Node);
            return result;
        }
        catch (KeyNotFoundException knf)
        {
            if (evaluator.SilentErrors)
            {
                return new EvalResult()
                {
                    Result = EvalResult.ResultType.ResultError,
                    Message = knf.Message
                };
            }
            throw;
        }
        catch (Exception ex)
        {
            if (evaluator.SilentErrors)
            {
                return new EvalResult()
                {
                    Result = EvalResult.ResultType.ResultError,
                    Message = ex.Message
                };
            }
            throw;
        }
    }

    private static AppliedRule CreateTrueResult(EvalResult result, ParsedRule rule)
    {
        return new AppliedRule(rule, result);
    }

    private static AppliedRule CreateFalseResult(EvalResult result, ParsedRule rule)
    {
        return AppliedRule.FalseResult(rule, result);
    }

    private static AppliedRule CreateErrorResult(EvalResult result, ParsedRule rule)
    {
        return AppliedRule.ErrorResult(rule, result);
    }
}
