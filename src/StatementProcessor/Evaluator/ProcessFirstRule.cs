using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;

namespace RightRule.StatementProcessor.Evaluator;

internal class ProcessFirstRule
{
    internal static AppliedRule EvaluateFirst(Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        EvalResult result = new();

        AppliedRule appliedRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            result = Evaluate(evaluator, rule, registeredDatasets);
            if (result.Result.Equals(EvalResult.ResultType.ResultTrue))
            {
                appliedRule = CreateAppliedRule(result, rule);
                break;
            }
            if (result.Result.Equals(EvalResult.ResultType.ResultError))
            {
                appliedRule = CreateAppliedRule(result, rule);
                break;
            }
            if (result.Result.Equals(EvalResult.ResultType.ResultTrue))
            {
                break;
            }
        }
        return appliedRule;
    }

    private static EvalResult Evaluate(Evaluator evaluator, ParsedRule rule, RegisteredDatasets datasets)
    {
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
                    Message = knf.Message,
                    StatementApplied = string.Empty
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
                    Message = ex.Message,
                    StatementApplied = string.Empty
                };
            }
            throw;
        }
    }

    private static AppliedRule CreateAppliedRule(EvalResult result, ParsedRule rule)
    {
        return new AppliedRule(rule, result);
    }
}
