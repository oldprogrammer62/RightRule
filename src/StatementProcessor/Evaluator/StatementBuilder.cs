using RightRule.Data;
using RightRule.StatementProcessor.Parser;
using RightRule.System.Common.Exceptions;
using System.Globalization;

namespace RightRule.StatementProcessor.Evaluator;

internal class StatementBuilder
{
    // New: Build a human-readable statement where identifiers are replaced with actual dataset values.
    // If an AST node is available it will be used to render the statement precisely; otherwise the original statement is returned.
    internal static Task<string> BuildAppliedStatement(RegisteredDatasets datasets, Node? node, string originalStatement)
    {
        return Task.Run(() =>
        {
            if (node is null)
                return originalStatement ?? string.Empty;

            return RenderNode(node, datasets);
        });
    }

    private static string RenderNode(Node node, RegisteredDatasets datasets)
    {
        return node switch
        {
            ConditionNode c => RenderCondition(c, datasets),
            BinaryNode b => $"({RenderNode(b.Left, datasets)} {b.LogicOp.ToUpperInvariant()} {RenderNode(b.Right, datasets)})",
            NotNode n => $"(NOT {RenderNode(n.Inner, datasets)})",
            _ => string.Empty
        };
    }

    private static string RenderCondition(ConditionNode c, RegisteredDatasets datasets)
    {
        var left = RenderOperand(c.Left, datasets);
        var right = RenderOperand(c.Right, datasets);
        return $"{left} {c.Operator} {right}";
    }

    private static string RenderOperand(Operand operand, RegisteredDatasets datasets)
    {
        switch (operand.Kind)
        {
            case OperandKind.Identifier:
                {
                    // operand.Raw is expected to be dataset.element
                    var value = ResolveIdentifier(datasets, operand.Raw);
                    return FormatValue(value);
                }
            case OperandKind.String:
                {
                    // string literal (raw already unquoted by tokenizer)
                    return FormatValue(operand.Raw);
                }
            case OperandKind.Number:
                {
                    // keep numeric textual representation, but normalize formatting
                    var parsed = Helper.ParseNumber(operand.Raw);
                    return FormatValue(parsed);
                }
            case OperandKind.Boolean:
                {
                    var parsed = Helper.ParseBoolean(operand.Raw);
                    return FormatValue(parsed);
                }
            default:
                return operand.Raw;
        }
    }

    private static object ResolveIdentifier(RegisteredDatasets datasets, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidIdentifierException(name, -1, $"Identifier '{name}' cannot be null or empty.");
        }
        if (name.Split('.').Length != 2)
        {
            throw new InvalidIdentifierException(name, -1, $"Identifier '{name}' must be in the format 'datasetName.identifierName'.");
        }

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

    private static string FormatValue(object? value)
    {
        if (value is null)
            return "null";

        if (value is string s)
        {
            // Escape backslashes and quotes
            var escaped = s.Replace("\\", "\\\\").Replace("\"", "\\\"");
            return $"\"{escaped}\"";
        }

        if (value is bool b)
            return b ? "true" : "false";

        if (value is IFormattable f)
            return f.ToString(null, CultureInfo.InvariantCulture) ?? value.ToString() ?? string.Empty;

        return value.ToString() ?? string.Empty;
    }
}
