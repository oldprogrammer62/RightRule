using RightRule.StatementProcessor.Parser;
using System.Text;

namespace RightRule.StatementProcessor;

internal static class StatementPathBuilder
{
    /// <summary>
    /// Produce a user-friendly ASCII tree representation of the AST for this parsed rule.
    /// Uses ├─ and └─ for branches and │ for vertical lines.
    /// Returns an empty string when <see cref="Node"/> is null.
    /// </summary>
    public static string BuildTreeText(Node node)
    {
        if (node is null) return string.Empty;

        var sb = new StringBuilder();
        Build(node, sb, string.Empty, true);
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
                case BinaryNode b:
                    var childrenB = new Node[] { b.Left, b.Right };
                    for (int i = 0; i < childrenB.Length; i++)
                    {
                        var child = childrenB[i];
                        if (child is null) continue;
                        Build(child, sb, childPrefix, i == childrenB.Length - 1);
                    }
                    break;

                case NotNode n:
                    if (n.Inner is not null)
                        Build(n.Inner, sb, childPrefix, true);
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

        static string Describe(Node node)
        {
            return node switch
            {
                BinaryNode b => $"Binary: {b.LogicOp}",
                NotNode _ => "NOT",
                ConditionNode c => $"Condition: {FormatOperand(c.Left)} {c.Operator} {FormatOperand(c.Right)}",
                _ => $"Unknown node: {node.GetType().Name}"
            };
        }

        static string FormatOperand(Operand op)
        {
            return op.Kind switch
            {
                OperandKind.Variable => $"Variable({op.Raw})",
                OperandKind.Identifier => $"Dataset.Identifier({op.Raw})",
                OperandKind.Number => $"Number({op.Raw})",
                OperandKind.String => $"String(\"{EscapeForDisplay(op.Raw)}\")",
                OperandKind.Boolean => $"Boolean({op.Raw})",
                _ => $"Operand({op.Raw})"
            };
        }

        static string EscapeForDisplay(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return raw ?? string.Empty;
            return raw.Replace("\"", "\\\"");
        }
    }
}
