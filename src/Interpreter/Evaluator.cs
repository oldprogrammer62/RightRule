using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;
using System.Globalization;

namespace RightRule.Interpreter;

public sealed class Evaluator
{
    private readonly RegisteredDatasets _registeredDatasets;
    private readonly Dictionary<string, IOperator> _operators;

    public Evaluator(RegisteredDatasets data, Dictionary<string, IOperator> Operators)
    {
        _registeredDatasets = data ?? new RegisteredDatasets();
        _operators = Operators ?? new Dictionary<string, IOperator> { };
    }

    public AppliedRule Evaluate(ParsedRules rules)
    {
        return Evaluate(rules, RulesToApply.First);
    }

    public AppliedRule Evaluate(ParsedRules rules, RulesToApply rulesToApply)
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

                var result = Evaluate(data, rule.Node, rule.Arguments);
                if (result)
                {
                    resultRule = new AppliedRule(rule, data.Name);
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

                var result = Evaluate(data, rule.Node, rule.Arguments);
                if (!result)
                {
                    resultRule = AppliedRule.DefaultRule();
                    break;
                }
                if (result)
                {
                    matchedRules.Add(rule);
                    resultRule = new AppliedRule(rule, data.Name, matchedRules);
                }
            }
        }
        return resultRule;
    }

    private bool Evaluate(RegisteredDataset data, Node? node, object? arguments) => node switch
    {
        ConditionNode c => EvaluateCondition(data, c, arguments),
        BinaryNode b => EvaluateBinary(data, b, arguments),
        NotNode n => !Evaluate(data, n.Inner, arguments),
        _ => throw new Exception("Unknown Node type.")
    };

    private bool EvaluateBinary(RegisteredDataset data, BinaryNode b, object? arguments)
    {
        if (b.LogicOp == "AND")
        {
            var left = Evaluate(data, b.Left, arguments);
            return left && Evaluate(data, b.Right, arguments);
        }
        if (b.LogicOp == "OR")
        {
            var left = Evaluate(data, b.Left, arguments);
            return left || Evaluate(data, b.Right, arguments);
        }
        throw new Exception($"Unknown logical operator: {b.LogicOp}");
    }

    private bool EvaluateCondition(RegisteredDataset data, ConditionNode c, object? arguments)
    {
        if (!_operators.TryGetValue(c.Operator, out var op))
            throw new Exception($"Unknown operator: {c.Operator}");

        var left = ResolveIdentifier(data, c.LeftIdentifier);
        var right = ResolveOperand(data, c.Right);
        return op.Process(left, right, arguments);
    }

    private object ResolveIdentifier(RegisteredDataset data, string name)
    {
        if (data.Data.TryGetValue(name, out var value))
            return value;
        throw new KeyNotFoundException($"Unknown variable '{name}' in data.");
    }

    private object ResolveOperand(RegisteredDataset data, Operand op)
    {
        return op.Kind switch
        {
            OperandKind.Identifier => ResolveIdentifier(data, op.Raw),
            OperandKind.Number => ParseNumber(op.Raw),
            OperandKind.Boolean => ParseBoolean(op.Raw),
            OperandKind.String => op.Raw,
            _ => op.Raw
        };
    }

    private static object ParseBoolean(string raw)
    {
        if (bool.TryParse(raw, out var value))
            return value;
        return raw;
    }

    private static object ParseNumber(string raw)
    {
        // Prefer int, fall back to double
        if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            return i;
        if (double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
            return d;
        return raw; // fallback: treat as string if parsing fails
    }
}
