using RightRule.StatementProcessor.Parser;
using System.Text;

namespace RightRule.RuleModels;

/// <summary>
/// A ParsedRule is a Rule that has been parsed and has an associated Node representing the parsed rule statement.
/// It is used during the evaluation process to evaluate the rule statement against the provided data.
/// The Node property allows the interpreter to traverse the abstract syntax tree (AST) of the parsed rule statement and evaluate it accordingly.
/// This class is internal
/// </summary>
public sealed class ParsedRule : Rule
{
    /// <summary>
    /// Indicates the outcome of the Parsing process.
    /// </summary>
    public required bool Success { get; init; }

    /// <summary>
    /// The Node property represents the parsed rule statement as an abstract syntax tree (AST).
    /// It allows the interpreter to traverse the AST and evaluate the rule statement.
    /// </summary>
    public required Node? Node { get; init; }

    /// <summary>
    /// When parsing fails during registration this holds the parser/tokenizer error message.
    /// </summary>
    public string? ParserMessage { get; init; }

    private ParsedRule() : base()
    {
        Success = false;
        Node = null;
        ParserMessage = string.Empty;
    }

    internal static ParsedRule SuccessResult(RuleDefinition ruleDefinition, Node node, string message)
    {
        return new ParsedRule()
        {
            Success = true,
            ParserMessage = message,
            Context = ruleDefinition.Context,
            Decision = ruleDefinition.Decision,
            DecisionData = ruleDefinition.DecisionData,
            Name = ruleDefinition.Name,
            Number = ruleDefinition.Number,
            Statement = ruleDefinition.Statement,
            OperatorConfig = ruleDefinition.OperatorConfig,
            Node = node
        };
    }

    internal static ParsedRule FailResult(RuleDefinition ruleDefinition, string message)
    {
        return new ParsedRule()
        {
            Success = false,
            ParserMessage = message,
            Context = ruleDefinition.Context,
            Decision = ruleDefinition.Decision,
            DecisionData = ruleDefinition.DecisionData,
            Name = ruleDefinition.Name,
            Number = ruleDefinition.Number,
            Statement = ruleDefinition.Statement,
            OperatorConfig = ruleDefinition.OperatorConfig,
            Node = null
        };
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {this.Number}");
        sb.AppendLine($"Name: {this.Name}");
        sb.AppendLine($"Context: {this.Context}");
        sb.AppendLine($"Statement: {this.Statement}");
        sb.AppendLine($"Configuration: {this.OperatorConfig}");
        sb.AppendLine($"Decision: {this.Decision}");
        sb.AppendLine($"DecisionData: {this.DecisionData}");
        return sb.ToString();
    }

    public async Task<string> ToTreeTextAsync()
    {
        return await Task.Run(BuildTreeTextAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Produce a user-friendly ASCII tree representation of the AST for this parsed rule.
    /// Uses ├─ and └─ for branches and │ for vertical lines.
    /// Returns an empty string when <see cref="Node"/> is null.
    /// </summary>
    private async Task<string> BuildTreeTextAsync()
    {
        if (Node is null) return string.Empty;

        var sb = new StringBuilder();
        await Build(Node, sb, string.Empty, true).ConfigureAwait(false);
        return sb.ToString();

        static Task Build(Node node, StringBuilder sb, string prefix, bool isLast)
        {
            return Task.Run(async () =>
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
                            await Build(child, sb, childPrefix, i == childrenB.Length - 1).ConfigureAwait(false);
                        }
                        break;

                    case NotNode n:
                        if (n.Inner is not null)
                            await Build(n.Inner, sb, childPrefix, true).ConfigureAwait(false);
                        break;

                    case ConditionNode:
                        // leaf
                        break;

                    default:
                        // unknown node types - nothing further
                        break;
                }
            });
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
                // OperandKind.Identifier => $"Identifier({op.Raw})",
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
