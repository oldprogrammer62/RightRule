using RightRule.Data;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Parser;
using System.Globalization;

namespace RightRule.LanguageProcessor.StatementBuilder;

public class StatementBuilder : IStatementBuilder
{
    private readonly IDataService _dataService;

    public StatementBuilder(IDataService dataService)
    {
        this._dataService = dataService;
    }

    // New: Build a human-readable statement where identifiers are replaced with actual dataset values.
    // If an AST node is available it will be used to render the statement precisely; otherwise the original statement is returned.
    public Task<string> BuildAppliedStatement(Node? node, string originalStatement)
    {
        return Task.Run(() =>
        {
            if (node is null)
                return originalStatement ?? string.Empty;

            return RenderNode(node);
        });
    }

    private string RenderNode(Node node)
    {
        return node switch
        {
            ConditionNode c => RenderCondition(c),
            BinaryNode b => $"({RenderNode(b.Left)} {b.LogicOp.ToUpperInvariant()} {RenderNode(b.Right)})",
            NotNode n => $"(NOT {RenderNode(n.Inner)})",
            _ => string.Empty
        };
    }

    private string RenderCondition(ConditionNode c)
    {
        var left = RenderOperand(c.Left);
        var right = RenderOperand(c.Right);
        return $"{left} {c.Operator} {right}";
    }

    private string RenderOperand(Operand operand)
    {
        switch (operand.Kind)
        {
            case OperandKind.Variable:
                {
                    var variable = EvaluatorHelper.ResolveVariable(operand.Raw, _dataService);
                    return FormatValue(variable);
                }
            case OperandKind.Identifier:
                {
                    // operand.Raw is expected to be dataset.element
                    var value = EvaluatorHelper.ResolveIdentifier(operand.Raw, _dataService);
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

    private string FormatValue(object? value)
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
