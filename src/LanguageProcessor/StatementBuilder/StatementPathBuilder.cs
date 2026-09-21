using RightRule.LanguageProcessor.Parser;
using System.Text;

namespace RightRule.LanguageProcessor.StatementBuilder;

internal static class StatementPathBuilder
{
    /// <summary>
    /// Produce a user-friendly ASCII tree representation of the AST for this parsed rule.
    /// Uses ├─ and └─ for branches and │ for vertical lines.
    /// Returns an empty string when <see cref="Node"/> is null.
    /// </summary>
    public static string BuildTreeText(Node Node)
    {
        if (Node is null) return string.Empty;

        var sb = new StringBuilder();
        Build(Node, sb, string.Empty, true);
        return sb.ToString();

        static void Build(Node node, StringBuilder sb, string prefix, bool isLast)
        {
            var branch = isLast ? "└─ " : "├─ ";
            sb.Append(prefix);
            sb.Append(branch);
            sb.AppendLine(Describe(node));

            // Prepare prefix for children
            var childPrefix = prefix + (isLast ? "   " : "│  ");

            switch (node)
            {
                case BinaryNode bNode:
                    var childrenB = new Node[] { bNode.Left, bNode.Right };
                    for (int i = 0; i < childrenB.Length; i++)
                    {
                        var child = childrenB[i];
                        if (child is null) continue;
                        Build(child, sb, childPrefix, i == childrenB.Length - 1);
                    }
                    break;

                case NotNode notNode:
                    if (notNode.Inner is not null)
                        Build(notNode.Inner, sb, childPrefix, true);
                    break;

                case ConditionNode:
                    // leaf
                    break;

                default:
                    // unknown node types - nothing further
                    break;
            }
            ;
        }

        static string Describe(Node Node)
        {
            return Node switch
            {
                BinaryNode b => $"Binary: {b.LogicOp}",
                NotNode _ => "NOT",
                ConditionNode c => $"Condition: {FormatOperand(c.Left)} {c.Operator} {FormatOperand(c.Right)}",
                _ => $"Unknown node: {Node.GetType().Name}"
            };
        }

        static string FormatOperand(Operand Op)
        {
            return Op.Kind switch
            {
                OperandKindEnum.VARIABLE => $"Variable({Op.Raw})",
                OperandKindEnum.IDENTIFIER => $"Dataset.Identifier({Op.Raw})",
                OperandKindEnum.NUMBER => $"Number({Op.Raw})",
                OperandKindEnum.STRING => $"String(\"{EscapeForDisplay(Op.Raw)}\")",
                OperandKindEnum.BOOLEAN => $"Boolean({Op.Raw})",
                _ => $"Operand({Op.Raw})"
            };
        }

        static string EscapeForDisplay(string Value)
        {
            if (string.IsNullOrEmpty(Value)) return Value ?? string.Empty;
            return Value.Replace("\"", "\\\"");
        }
    }
}
