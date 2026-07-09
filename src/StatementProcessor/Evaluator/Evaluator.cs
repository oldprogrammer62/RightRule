using RightRule.Data;
using RightRule.System.Common;

namespace RightRule.StatementProcessor.Evaluator;

public sealed class Evaluator
{
    private readonly Dictionary<string, IOperator> _operators;

    private readonly bool _silentErrors;
    internal bool SilentErrors => _silentErrors;

    internal Evaluator(Dictionary<string, IOperator> Operators, bool SilentErrors = false)
    {
        _operators = Operators ?? new Dictionary<string, IOperator> { };
        _silentErrors = SilentErrors;
    }

    internal bool Evaluate(RegisteredDatasets datasets, RegisteredDataset dataset, Node? node) => node switch
    {
        ConditionNode c => EvaluateCondition(datasets, dataset, c),
        BinaryNode b => EvaluateBinary(datasets, dataset, b),
        NotNode n => !Evaluate(datasets, dataset, n.Inner),
        _ => throw new Exception("Unknown Node type.")
    };

    private bool EvaluateBinary(RegisteredDatasets datasets, RegisteredDataset dataset, BinaryNode b)
    {
        if (b.LogicOp.ToLower() == "and")
        {
            var left = Evaluate(datasets, dataset, b.Left);
            return left && Evaluate(datasets, dataset, b.Right);
        }
        if (b.LogicOp.ToLower() == "or")
        {
            var left = Evaluate(datasets, dataset, b.Left);
            return left || Evaluate(datasets, dataset, b.Right);
        }
        throw new Exception($"Unknown logical operator: {b.LogicOp}");
    }

    private bool EvaluateCondition(RegisteredDatasets datasets, RegisteredDataset dataset, ConditionNode c)
    {
        if (!_operators.TryGetValue(c.Operator, out var op))
            throw new KeyNotFoundException($"Unknown operator: {c.Operator}");

        var left = ResolveOperandValue(datasets, dataset, c.Left);
        var right = ResolveOperandValue(datasets, dataset, c.Right);
        return op.Process(left!, right!);
    }

    private static object? ResolveIdentifier(RegisteredDataset dataset, string name)
    {
        if (!dataset.Data.TryGetValue(name, out _))
        {
            return null;
        }
        dataset.Data.TryGetValue(name, out var result);
        return result!;
    }

    private static object ResolveIdentifierWithDot(RegisteredDatasets datasets, string name)
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

    private object? ResolveOperandValue(RegisteredDatasets datasets, RegisteredDataset dataset, Operand op)
    {
        return ResolveOperand(datasets, dataset, op);
    }

    private static object? ResolveOperand(RegisteredDatasets datasets, RegisteredDataset dataset, Operand op)
    {
        object opValue = ResolveOperatorValue(op);

        return op.Kind switch
        {
            OperandKind.Identifier => opValue is string sId ? ResolveIdentifier(dataset, sId) : ResolveIdentifier(dataset, op.Raw),
            OperandKind.IdentifierWithDot => opValue is string sId ? ResolveIdentifierWithDot(datasets, sId) : ResolveIdentifierWithDot(datasets, op.Raw),
            OperandKind.Number => opValue is string sNum ? ParseNumber(sNum) : opValue,
            OperandKind.Boolean => opValue is string sBool ? ParseBoolean(sBool) : opValue,
            OperandKind.String => opValue is string s ? s : (opValue?.ToString() ?? string.Empty),
            _ => opValue
        };
    }

    private static string ResolveOperatorValue(Operand operand)
    {
        return operand.Raw;
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
