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

    public AppliedRule ProcessRule(ParsedRules rules)
    {
        return EvaluateFirst(rules.RightRules);
    }

    public List<AppliedRule> ProcessRules(ParsedRules rules)
    {
        return Evaluate(rules.RightRules, RulesToApply.All);
    }

    public List<AppliedRule> ProcessRules(ParsedRules rules, RulesToApply rulesToApply)
    {
        return Evaluate(rules.RightRules, rulesToApply);
    }

    private List<AppliedRule> Evaluate(List<ParsedRule> rules, RulesToApply rulesToApply)
    {
        switch (rulesToApply)
        {
            case RulesToApply.First:
                return [EvaluateFirst(rules)];

            case RulesToApply.All:
                return EvaluateAll(rules);

            default:
                return [AppliedRule.DefaultRule($"Unknown RulesToApply value: {rulesToApply}")];
        }
    }

    private AppliedRule EvaluateFirst(List<ParsedRule> rules)
    {
        bool result = false;

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

                result = _evaluator.Evaluate(data, rule.Node, rule.Arguments);
                var statement = Helper.ResolveStatement(rule.Statement, rule.Arguments);
                resultRule = new AppliedRule(rule, data.Name, statement, result);
                break;
            }
            if (result)
            {
                break;
            }
        }
        return resultRule;
    }

    private List<AppliedRule> EvaluateAll(List<ParsedRule> rules)
    {
        AppliedRule resultRule = AppliedRule.DefaultRule();
        List<AppliedRule> appliedRules = [];

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
                var statement = Helper.ResolveStatement(rule.Statement, rule.Arguments);
                var appliedRule = new AppliedRule(rule, data.Name, statement, result);
                appliedRules.Add(appliedRule);
            }
        }
        return appliedRules;
    }
}
