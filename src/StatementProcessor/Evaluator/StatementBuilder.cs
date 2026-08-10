using RightRule.Data;
using RightRule.StatementProcessor.Parser;
using RightRule.System.Common.Exceptions;
using System.Globalization;

namespace RightRule.StatementProcessor.Evaluator;

internal class StatementBuilder
{
    // New: Build a human-readable statement where identifiers are replaced with actual dataset values.
    // If an AST node is available it will be used to render the statement precisely; otherwise the original statement is returned.
    internal static Task<string> BuildAppliedStatement(RegisteredDatasets? datasets, Node? node, string originalStatement, object? OptionalData)
    {
        return Task.Run(() =>
        {
            if (node is null)
                return originalStatement ?? string.Empty;

            return RenderNode(node, datasets, OptionalData);
        });
    }

    private static string RenderNode(Node node, RegisteredDatasets? Datasets, object? OptionalData)
    {
        return node switch
        {
            ConditionNode c => RenderCondition(c, Datasets, OptionalData),
            BinaryNode b => $"({RenderNode(b.Left, Datasets, OptionalData)} {b.LogicOp.ToUpperInvariant()} {RenderNode(b.Right, Datasets, OptionalData)})",
            NotNode n => $"(NOT {RenderNode(n.Inner, Datasets, OptionalData)})",
            _ => string.Empty
        };
    }

    private static string RenderCondition(ConditionNode c, RegisteredDatasets? Datasets, object? OptionalData)
    {
        var left = RenderOperand(c.Left, Datasets, OptionalData);
        var right = RenderOperand(c.Right, Datasets, OptionalData);
        return $"{left} {c.Operator} {right}";
    }

    private static string RenderOperand(Operand operand, RegisteredDatasets? Datasets, object? OptionalData)
    {
        switch (operand.Kind)
        {
            case OperandKind.Identifier:
                {
                    // operand.Raw is expected to be dataset.element
                    var value = ResolveIdentifier(Datasets, OptionalData, operand.Raw);
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

    private static object? ResolveIdentifier(RegisteredDatasets? Datasets, object? ObjectReference, string Identifier)
    {
        if (string.IsNullOrEmpty(Identifier))
        {
            throw new InvalidIdentifierException(Identifier, -1, $"Identifier '{Identifier}' cannot be null or empty.");
        }
        if (Identifier.Split('.').Length != 2)
        {
            throw new InvalidIdentifierException(Identifier, -1, $"Identifier '{Identifier}' must be in the format 'datasetName.identifierName'.");
        }

        var split = Identifier.Split('.');
        var parent = split[0];
        var child = split[1];
        var ds = Datasets?.GetDataset(parent);
        if (ds is not null)
        {
            return ResolveIdentifier(ds, child);
        }
        return ResolveIdentifier(ObjectReference, parent, child);
    }

    private static object? ResolveIdentifier(object? ObjectReference, string ObjectName, string PropertyName)
    {
        if (string.IsNullOrEmpty(ObjectName))
        {
            throw new KeyNotFoundException($"Unknown Object.");
        }
        if (string.IsNullOrEmpty(PropertyName))
        {
            throw new KeyNotFoundException($"Unknown Property.");
        }
        if (ObjectReference is null)
        {
            throw new KeyNotFoundException("Object Reference not set.");
        }

        var type = ObjectReference.GetType();
        var property = type.GetProperty(PropertyName);
        if (property is not null)
        {
            return property.GetValue(ObjectReference);
        }
        else
        {
            throw new KeyNotFoundException($"Unknown Property '{PropertyName}' in {type.Name}.");
        }
    }

    private static object? ResolveIdentifier(RegisteredDataset? Dataset, string KeyName)
    {
        if (string.IsNullOrEmpty(KeyName))
        {
            throw new KeyNotFoundException($"Unknown variable '{KeyName}'.");
        }
        if (Dataset is null)
        {
            throw new KeyNotFoundException($"Unknown dataset '{Dataset}'");
        }
        if (!Dataset.Data.TryGetValue(KeyName, out _))
        {
            throw new KeyNotFoundException($"Unknown variable '{KeyName}' in Registered Dataset {Dataset.Name}.");
        }

        Dataset.Data.TryGetValue(KeyName, out var result);
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
