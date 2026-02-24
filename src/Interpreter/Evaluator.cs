using RightRule.Rules;
using RightRule.System.Common;
using System.Globalization;

namespace RightRule.Interpreter;

public sealed class Evaluator
{
    private readonly IReadOnlyDictionary<string, object> _context;
    private readonly IReadOnlyDictionary<string, IOperator> _operators;

    public Evaluator(
        IReadOnlyDictionary<string, object> context,
        IEnumerable<IOperator> operators)
    {
        _context = context ?? new Dictionary<string, object>();

        var dict = new Dictionary<string, IOperator>(StringComparer.OrdinalIgnoreCase);
        foreach (var op in operators)
        {
            foreach (var name in op.Identifiers)
            {
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                dict[name] = op;
            }
        }
        _operators = dict;
    }

    public AppliedRule Evaluate(ParsedRules rules)
    {
        return Evaluate(rules.RightRules);
    }

    public AppliedRule Evaluate(List<ParsedRule> rules)
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

    public bool Evaluate(Node? node) => node switch
    {
        ConditionNode c => EvaluateCondition(c),
        BinaryNode b => EvaluateBinary(b),
        NotNode n => !Evaluate(n.Inner),
        _ => throw new Exception("Unknown AST node.")
    };

    private bool EvaluateBinary(BinaryNode b)
    {
        // short-circuit semantics
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
        if (!_operators.TryGetValue(c.Operator, out var op))
            throw new Exception($"Unknown operator: {c.Operator}");

        var left = ResolveIdentifier(c.LeftIdentifier);
        var right = ResolveOperand(c.Right);
        return op.Process(left, right);
    }

    private object ResolveIdentifier(string name)
    {
        if (_context.TryGetValue(name, out var value))
            return value;
        throw new KeyNotFoundException($"Unknown variable '{name}' in context.");
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
