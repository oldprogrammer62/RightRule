using RightRule.Data;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Evaluator;

public sealed class Evaluator
{
    private readonly Dictionary<string, IOperator> _operators;

    public Evaluator(Dictionary<string, IOperator> Operators)
    {
        _operators = Operators ?? new Dictionary<string, IOperator> { };
    }

    internal bool Evaluate(RegisteredDataset data, Node? node, Dictionary<string, object>? arguments) => node switch
    {
        ConditionNode c => EvaluateCondition(data, c, arguments),
        BinaryNode b => EvaluateBinary(data, b, arguments),
        NotNode n => !Evaluate(data, n.Inner, arguments),
        _ => throw new Exception("Unknown Node type.")
    };

    private bool EvaluateBinary(RegisteredDataset data, BinaryNode b, Dictionary<string, object>? arguments)
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

    private bool EvaluateCondition(RegisteredDataset data, ConditionNode c, Dictionary<string, object>? arguments)
    {
        if (!_operators.TryGetValue(c.Operator, out var op))
            throw new Exception($"Unknown operator: {c.Operator}");

        var left = ResolveIdentifier(data, c.LeftIdentifier);
        var right = ResolveOperandValue(data, c.Right, arguments);
        return op.Process(left, right);
    }

    private object ResolveIdentifier(RegisteredDataset data, string name)
    {
        if (data.Data.TryGetValue(name, out var value))
            return value;
        throw new KeyNotFoundException($"Unknown variable '{name}' in data.");
    }

    private object ResolveOperandValue(RegisteredDataset data, Operand op, Dictionary<string, object>? arguments)
    {
        if (arguments == null)
        {
            return ResolveOperand(data, op);
        }
        return ResolveOperand(data, op, arguments);
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

    private object ResolveOperand(RegisteredDataset data, Operand op, Dictionary<string, object>? arguments)
    {
        object opValue = ResolveOperatorValue(op, arguments!);

        return op.Kind switch
        {
            OperandKind.Identifier => opValue is string sId ? ResolveIdentifier(data, sId) : ResolveIdentifier(data, op.Raw),
            OperandKind.Number => opValue is string sNum ? ParseNumber(sNum) : opValue,
            OperandKind.Boolean => opValue is string sBool ? ParseBoolean(sBool) : opValue,
            OperandKind.Placeholder => opValue is string sPlace ? ParsePlaceholder(sPlace) : opValue,
            OperandKind.String => opValue is string s ? s : (opValue?.ToString() ?? string.Empty),
            _ => opValue
        };
    }

    private object ResolveOperatorValue(Operand operand, Dictionary<string, object> Arguments)
    {
        return Helper.ResolveOperatorValue(operand, Arguments);
    }

    private static object ParsePlaceholder(string raw)
    {
        return Helper.ParsePlaceholder(raw);
    }

    private static object ParseBoolean(string raw)
    {
        return Helper.ParseBoolean(raw);
    }

    private static object ParseNumber(string raw)
    {
        return Helper.ParseNumber(raw);
    }
}
