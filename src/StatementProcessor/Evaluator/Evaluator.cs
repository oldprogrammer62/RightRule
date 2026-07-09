using RightRule.Data;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Evaluator;

public sealed class Evaluator
{
    private readonly Dictionary<string, IOperator> _operators;

    private readonly bool _silentErrors;
    public bool SilentErrors => _silentErrors;

    public Evaluator(Dictionary<string, IOperator> Operators, bool SilentErrors = false)
    {
        _operators = Operators ?? new Dictionary<string, IOperator> { };
        _silentErrors = SilentErrors;
    }

    internal bool Evaluate(RegisteredDatasets datasets, RegisteredDataset dataset, Node? node, Dictionary<string, object>? arguments) => node switch
    {
        ConditionNode c => EvaluateCondition(datasets, dataset, c, arguments),
        BinaryNode b => EvaluateBinary(datasets, dataset, b, arguments),
        NotNode n => !Evaluate(datasets, dataset, n.Inner, arguments),
        _ => throw new Exception("Unknown Node type.")
    };

    private bool EvaluateBinary(RegisteredDatasets datasets, RegisteredDataset dataset, BinaryNode b, Dictionary<string, object>? arguments)
    {
        if (b.LogicOp == "AND")
        {
            var left = Evaluate(datasets, dataset, b.Left, arguments);
            return left && Evaluate(datasets, dataset, b.Right, arguments);
        }
        if (b.LogicOp == "OR")
        {
            var left = Evaluate(datasets, dataset, b.Left, arguments);
            return left || Evaluate(datasets, dataset, b.Right, arguments);
        }
        throw new Exception($"Unknown logical operator: {b.LogicOp}");
    }

    private bool EvaluateCondition(RegisteredDatasets datasets, RegisteredDataset dataset, ConditionNode c, Dictionary<string, object>? arguments)
    {
        if (!_operators.TryGetValue(c.Operator, out var op))
            throw new KeyNotFoundException($"Unknown operator: {c.Operator}");

        var left = ResolveOperandValue(datasets, dataset, c.Left, arguments);
        var right = ResolveOperandValue(datasets, dataset, c.Right, arguments);
        return op.Process(left, right);
    }

    private object ResolveIdentifier(RegisteredDataset dataset, string name)
    {
        if (!dataset.Data.TryGetValue(name, out _))
            throw new KeyNotFoundException($"Unknown variable '{name}' in Registered Dataset {dataset.Name}.");

        dataset.Data.TryGetValue(name, out var result);
        return result!;
    }

    private object ResolveIdentifierWithDot(RegisteredDatasets datasets, string name)
    {
        var split = name.Split('.');
        var datasetname = split[0];
        var identifiername = split[1];
        var ds = datasets.GetDataset(datasetname);
        if (ds is null)
        {
            throw new KeyNotFoundException($"Unknown dataset name '{datasetname}'.");
        }

        if (!ds.Data.TryGetValue(identifiername, out _))
        {
            throw new KeyNotFoundException($"Unknown variable '{identifiername}' in Registered Dataset {datasetname}.");
        }

        ds.Data.TryGetValue(identifiername, out var result);
        return result!;
    }

    private object ResolveOperandValue(RegisteredDatasets datasets, RegisteredDataset dataset, Operand op, Dictionary<string, object>? arguments)
    {
        if (arguments == null)
        {
            return ResolveOperand(datasets, dataset, op);
        }
        return ResolveOperand(datasets, dataset, op, arguments);
    }

    private object ResolveOperand(RegisteredDatasets datasets, RegisteredDataset dataset, Operand op)
    {
        return op.Kind switch
        {
            OperandKind.IdentifierWithDot => ResolveIdentifierWithDot(datasets, op.Raw),
            OperandKind.Identifier => ResolveIdentifier(dataset, op.Raw),
            OperandKind.Number => ParseNumber(op.Raw),
            OperandKind.Boolean => ParseBoolean(op.Raw),
            OperandKind.String => op.Raw,
            _ => op.Raw
        };
    }

    private object ResolveOperand(RegisteredDatasets datasets, RegisteredDataset dataset, Operand op, Dictionary<string, object>? arguments)
    {
        object opValue = ResolveOperatorValue(op, arguments!);

        return op.Kind switch
        {
            OperandKind.IdentifierWithDot => opValue is string sId ? ResolveIdentifierWithDot(datasets, sId) : ResolveIdentifierWithDot(datasets, op.Raw),
            OperandKind.Identifier => opValue is string sId ? ResolveIdentifier(dataset, sId) : ResolveIdentifier(dataset, op.Raw),
            OperandKind.Number => opValue is string sNum ? ParseNumber(sNum) : opValue,
            OperandKind.Boolean => opValue is string sBool ? ParseBoolean(sBool) : opValue,
            OperandKind.String => opValue is string s ? s : (opValue?.ToString() ?? string.Empty),
            OperandKind.Placeholder => opValue is string sPlace ? ParsePlaceholder(sPlace) : opValue,
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
