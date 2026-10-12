using RightRule.DataServices;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.Parser;
using System.Globalization;

namespace RightRule.LanguageProcessor.StatementBuilder;

internal class StatementBuilder : IStatementBuilder
{
    private readonly IDataService _dataService;

    public StatementBuilder(IDataService dataService)
    {
        this._dataService = dataService;
    }

    // New: Build a human-readable statement where identifiers are replaced with actual dataset values.
    // If an AST node is available it will be used to render the statement precisely; otherwise the original statement is returned.
    public Task<string> BuildAppliedStatement(Node? Node, string Statement)
    {
        return Task.Run(() =>
        {
            if (Node is null)
                return Statement ?? string.Empty;

            return RenderNode(Node);
        });
    }

    private string RenderNode(Node Node)
    {
        return Node switch
        {
            ConditionNode c => RenderCondition(c),
            BinaryNode b => $"({RenderNode(b.Left)} {b.LogicOp.ToUpperInvariant()} {RenderNode(b.Right)})",
            NotNode n => $"(NOT {RenderNode(n.Inner)})",
            _ => string.Empty
        };
    }

    private string RenderCondition(ConditionNode Node)
    {
        var left = RenderOperand(Node.Left);
        var right = RenderOperand(Node.Right);
        return $"{left} {Node.Operator} {right}";
    }

    private string RenderOperand(Operand Op)
    {
        switch (Op.Kind)
        {
            case OperandKindEnum.VARIABLE:
                {
                    var variable = EvaluatorHelper.ResolveVariable(Op.Raw, _dataService);
                    return FormatValue(variable);
                }
            case OperandKindEnum.IDENTIFIER:
                {
                    // operand.Raw is expected to be dataset.element
                    var value = EvaluatorHelper.ResolveIdentifier(Op.Raw, _dataService);
                    return FormatValue(value);
                }
            case OperandKindEnum.STRING:
                {
                    // string literal (raw already unquoted by tokenizer)
                    return FormatValue(Op.Raw);
                }
            case OperandKindEnum.NUMBER:
                {
                    // keep numeric textual representation, but normalize formatting
                    var parsed = Helper.ParseNumber(Op.Raw);
                    return FormatValue(parsed);
                }
            case OperandKindEnum.BOOLEAN:
                {
                    var parsed = Helper.ParseBoolean(Op.Raw);
                    return FormatValue(parsed);
                }
            case OperandKindEnum.ARRAY:
                {
                    return FormatValue(ArrayLiteralParser.Parse(Op.Raw));
                }
            default:
                return Op.Raw;
        }
    }

    private string FormatValue(object? Value)
    {
        if (Value is null)
            return "null";

        if (Value is string s)
        {
            // Escape backslashes and quotes
            var escaped = s.Replace("\\", "\\\\").Replace("\"", "\\\"");
            return $"\"{escaped}\"";
        }

        if (Value is bool b)
            return b ? "true" : "false";

        if (Value is Array array)
        {
            var elements = new List<string>();
            foreach (var element in array)
                elements.Add(FormatValue(element));

            return $"[{string.Join(", ", elements)}]";
        }

        if (Value is IFormattable f)
            return f.ToString(null, CultureInfo.InvariantCulture) ?? Value.ToString() ?? string.Empty;

        return Value.ToString() ?? string.Empty;
    }
}
