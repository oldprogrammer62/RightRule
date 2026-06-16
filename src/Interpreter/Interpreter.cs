using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule.Interpreter;

public sealed class Interpreter
{
    private readonly RegisteredDatasets _registeredDatasets;
    private readonly Dictionary<string, IOperator> _operators;
    private readonly Evaluator _evaluator;

    public Interpreter(RegisteredDatasets data, Dictionary<string, IOperator> operators)
    {
        _registeredDatasets = data ?? new RegisteredDatasets();
        _operators = operators ?? new Dictionary<string, IOperator> { };
        _evaluator = new Evaluator(_operators);
    }

    public AppliedRule ProcessRules(ParsedRules rules)
    {
        return ProcessRules(rules, RulesToApply.First);
    }

    public AppliedRule ProcessRules(ParsedRules rules, RulesToApply rulesToApply)
    {
        return Evaluate(rules.RightRules, rulesToApply);
    }

    private AppliedRule Evaluate(List<ParsedRule> rules, RulesToApply rulesToApply)
    {
        switch (rulesToApply)
        {
            case RulesToApply.First:
                return EvaluateFirst(rules);

            case RulesToApply.All:
                return EvaluateAll(rules);

            default:
                return AppliedRule.DefaultRule($"Unknown RulesToApply value: {rulesToApply}");
        }
    }

    private AppliedRule EvaluateFirst(List<ParsedRule> rules)
    {
        bool success = false;

        AppliedRule resultRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            for (var i = 0; i < _registeredDatasets.Count; i++)
            {
                var data = _registeredDatasets[i];
                if (data == null)
                {
                    continue;
                }

                var result = _evaluator.Evaluate(data, rule.Node, rule.Arguments);
                if (result)
                {
                    resultRule = new AppliedRule(rule, data.Name, "Statement", result);
                    success = true;
                    break;
                }
            }
            if (success)
            {
                break;
            }
        }
        return resultRule;
    }

    private AppliedRule EvaluateAll(List<ParsedRule> rules)
    {
        AppliedRule resultRule = AppliedRule.DefaultRule();
        List<ParsedRule> matchedRules = [];

        foreach (var rule in rules)
        {
            for (var i = 0; i < _registeredDatasets.Count; i++)
            {
                var data = _registeredDatasets[i];
                if (data == null)
                {
                    continue;
                }

                var result = _evaluator.Evaluate(data, rule.Node, rule.Arguments);
                if (!result)
                {
                    resultRule = AppliedRule.DefaultRule();
                    break;
                }
                if (result)
                {
                    matchedRules.Add(rule);
                    resultRule = new AppliedRule(rule, data.Name, "statement", result);
                }
            }
        }
        return resultRule;
    }
}
