using RightRule.Data;
using RightRule.Models;
using RightRule.Rules;

namespace RightRule.StatementProcessor.Evaluator;

internal class ProcessFirstRule
{
    internal static AppliedRule EvaluateFirst(Evaluator evaluator, List<ParsedRule> rules, RegisteredDatasets registeredDatasets)
    {
        AppliedRule appliedRule = AppliedRule.DefaultRule();
        bool complete = false;

        foreach (var rule in rules)
        {
            EvalResult result = Evaluate(evaluator, rule, registeredDatasets);
            switch (result.Result)
            {
                case EvalResult.ResultType.ResultTrue:
                    appliedRule = CreateTrueResult(result, rule);
                    complete = true;
                    break;

                case EvalResult.ResultType.ResultFalse:
                    appliedRule = CreateFalseResult(result, rule);
                    continue;

                case EvalResult.ResultType.ResultError:
                    appliedRule = CreateErrorResult(result, rule);
                    break;

                default:
                    throw new InvalidOperationException($"Unexpected result type: {result.Result}");
            }
            if (complete)
            {
                break;
            }
        }
        return appliedRule;
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
