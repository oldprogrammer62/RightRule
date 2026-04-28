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
                throw new Exception($"Unknown RulesToApply value: {rulesToApply}");
        }
    }

    private AppliedRule EvaluateFirst(List<ParsedRule> rules)
    {
        AppliedRule resultRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            var result = Evaluate(rule.Node, rule.Arguments);
            if (result)
            {
                resultRule = new AppliedRule(rule);
                break;
            }
        }
        return resultRule;
    }

    private AppliedRule EvaluateAll(List<ParsedRule> rules)
    {
        AppliedRule resultRule = AppliedRule.DefaultRule();
        foreach (var rule in rules)
        {
            var result = Evaluate(rule.Node, rule.Arguments);
            if (!result)
            {
                resultRule = AppliedRule.DefaultRule();
                break;
            }
            if (result)
            {
                resultRule = new AppliedRule(rule);
            }
        }
        return resultRule;
    }

    private bool Evaluate(Node? node, object? arguments) => node switch
    {
        ConditionNode c => EvaluateCondition(c, arguments),
        BinaryNode b => EvaluateBinary(b, arguments),
        NotNode n => !Evaluate(n.Inner, arguments),
        _ => throw new Exception("Unknown Node type.")
    };

    private bool EvaluateBinary(BinaryNode b, object? arguments)
    {
        if (b.LogicOp == "AND")
        {
            var left = Evaluate(b.Left, arguments);
            return left && Evaluate(b.Right, arguments);
        }
        if (b.LogicOp == "OR")
        {
            var left = Evaluate(b.Left, arguments);
            return left || Evaluate(b.Right, arguments);
        }
        throw new Exception($"Unknown logical operator: {b.LogicOp}");
    }

    private bool EvaluateCondition(ConditionNode c, object? arguments)
    {
        if (!_operators.TryGetValue(c.Operator, out var op))
            throw new Exception($"Unknown operator: {c.Operator}");

        var left = ResolveIdentifier(c.LeftIdentifier);
        var right = ResolveOperand(c.Right);
        return op.Process(left, right, arguments);
    }

    private object ResolveIdentifier(string name)
    {
        if (_registeredDatasets != null)
        {
            for (var i = 0; i < _registeredDatasets.Count; i++)
            {
                if (_registeredDatasets[i] == null)
                    continue;
                if (_registeredDatasets[i]!.Data.TryGetValue(name, out var value))
                    return value;
            }
        }
        throw new KeyNotFoundException($"Unknown variable '{name}' in data.");
    }

    private object ResolveOperand(Operand op)
    {
        return op.Kind switch
        {
            OperandKind.Identifier => ResolveIdentifier(op.Raw),
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
