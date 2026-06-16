using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;
using System.Globalization;

namespace RightRule.Interpreter;

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
        string opValue = ResolveOperatorValue(op, arguments!);

        return op.Kind switch
        {
            OperandKind.Identifier => ResolveIdentifier(data, opValue),
            OperandKind.Number => ParseNumber(opValue),
            OperandKind.Boolean => ParseBoolean(opValue),
            OperandKind.Placeholder => ParsePlaceholder(opValue),
            OperandKind.String => opValue,
            _ => opValue
        };
    }

    private string ResolveOperatorValue(Operand operand, Dictionary<string, object> Arguments)
    {
        char brace = '{';
        if (operand.Raw.Contains(brace))
        {
            return GetOperatorValue(operand.Raw, Arguments);
        }
        return operand.Raw;
    }

    private string GetOperatorValue(string Op, Dictionary<string, object> Arguments)
    {
        var operatorName = GetOperatorName(Op);

        if (Arguments.TryGetValue(operatorName, out var value))
        {
            return value.ToString() ?? string.Empty;
        }
        return string.Empty;
    }

    private string GetOperatorName(string Op)
    {
        char[] StartChar = { '{' };
        char[] EndChar = { '}' };

        return Op.TrimStart(StartChar).TrimEnd(EndChar);
    }

    private static object ParsePlaceholder(string raw)
    {
        if (bool.TryParse(raw, out var value))
            return value;

        // Prefer int, fall back to double
        if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            return i;
        if (double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
            return d;

        // Assume string
        return raw;
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
