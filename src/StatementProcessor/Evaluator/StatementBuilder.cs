using RightRule.DataRegistration;
using RightRule.StatementProcessor.Parser;
using System.Globalization;

namespace RightRule.StatementProcessor.Evaluator;

internal static class StatementBuilder
{
    // New: Build a human-readable statement where identifiers are replaced with actual dataset values.
    // If an AST node is available it will be used to render the statement precisely; otherwise the original statement is returned.
    internal static Task<string> BuildAppliedStatement(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Node? node, string originalStatement)
    {
        return Task.Run(() =>
        {
            if (node is null)
                return originalStatement ?? string.Empty;

            return RenderNode(node, Datasets, DataObjects);
        });
    }

    private static string RenderNode(Node node, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects)
    {
        return node switch
        {
            ConditionNode c => RenderCondition(c, Datasets, DataObjects),
            BinaryNode b => $"({RenderNode(b.Left, Datasets, DataObjects)} {b.LogicOp.ToUpperInvariant()} {RenderNode(b.Right, Datasets, DataObjects)})",
            NotNode n => $"(NOT {RenderNode(n.Inner, Datasets, DataObjects)})",
            _ => string.Empty
        };
    }

    private static string RenderCondition(ConditionNode c, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects)
    {
        var left = RenderOperand(c.Left, Datasets, DataObjects);
        var right = RenderOperand(c.Right, Datasets, DataObjects);
        return $"{left} {c.Operator} {right}";
    }

    private static string RenderOperand(Operand operand, IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects)
    {
        switch (operand.Kind)
        {
            case OperandKind.Variable:
                {
                    var variable = EvaluatorHelper.ResolveVariable(DataObjects, operand.Raw);
                    return FormatValue(variable);
                }
            case OperandKind.Identifier:
                {
                    // operand.Raw is expected to be dataset.element
                    var value = EvaluatorHelper.ResolveIdentifier(Datasets, DataObjects, operand.Raw);
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
