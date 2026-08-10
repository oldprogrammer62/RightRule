using RightRule.Data;
using RightRule.StatementProcessor.Parser;

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
                    var value = EvaluatorHelper.ResolveIdentifier(Datasets, OptionalData, operand.Raw);
                    return EvaluatorHelper.FormatValue(value);
                }
            case OperandKind.String:
                {
                    // string literal (raw already unquoted by tokenizer)
                    return EvaluatorHelper.FormatValue(operand.Raw);
                }
            case OperandKind.Number:
                {
                    // keep numeric textual representation, but normalize formatting
                    var parsed = Helper.ParseNumber(operand.Raw);
                    return EvaluatorHelper.FormatValue(parsed);
                }
            case OperandKind.Boolean:
                {
                    var parsed = Helper.ParseBoolean(operand.Raw);
                    return EvaluatorHelper.FormatValue(parsed);
                }
            default:
                return operand.Raw;
        }
    }
}
