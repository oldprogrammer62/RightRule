using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;
using System.Globalization;

namespace RightRule.Interpreter;

public sealed class Evaluator
{
    private readonly RegisteredDatasets _registeredDatasets;
    private readonly Dictionary<string, IOperator> _operatorNames;

    public Evaluator(
    RegisteredDatasets data,
    IEnumerable<IOperator> operators)
    {
        _registeredDatasets = data ?? new RegisteredDatasets();

        var operatorNames = new Dictionary<string, IOperator>(StringComparer.OrdinalIgnoreCase);
        foreach (var op in operators)
        {
            foreach (var name in op.Identifiers)
            {
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                operatorNames[name] = op;
            }
        }
        _operatorNames = operatorNames;
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
            var result = Evaluate(rule.Node);
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
            var result = Evaluate(rule.Node);
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

    private bool Evaluate(Node? node) => node switch
    {
        ConditionNode c => EvaluateCondition(c),
        BinaryNode b => EvaluateBinary(b),
        NotNode n => !Evaluate(n.Inner),
        _ => throw new Exception("Unknown AST node.")
    };

    private bool EvaluateBinary(BinaryNode b)
    {
        if (b.LogicOp == "AND")
        {
            var left = Evaluate(b.Left);
            return left && Evaluate(b.Right);
        }
        if (b.LogicOp == "OR")
        {
            var left = Evaluate(b.Left);
            return left || Evaluate(b.Right);
        }
        throw new Exception($"Unknown logical operator: {b.LogicOp}");
    }

    private bool EvaluateCondition(ConditionNode c)
    {
        if (!_operatorNames.TryGetValue(c.Operator, out var op))
            throw new Exception($"Unknown operator: {c.Operator}");

        var left = ResolveIdentifier(c.LeftIdentifier);
        var right = ResolveOperand(c.Right);
        return op.Process(left, right);
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
