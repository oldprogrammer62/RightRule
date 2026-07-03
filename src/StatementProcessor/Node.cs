namespace RightRule.StatementProcessor;

/// <summary>
/// Node is the base class for all nodes in the abstract syntax tree (AST)
/// A Node represents the parsed rule statement.
/// </summary>
public abstract class Node { }

public enum OperandKind { Identifier, Number, String, Boolean, Placeholder }

public sealed class Operand
{
    public OperandKind Kind { get; }
    public string Raw { get; }
    public Operand(OperandKind kind, string raw)
    {
        Kind = kind;
        Raw = raw;
    }
}

/// <summary>
/// Represents a simple condition in the rule statement.
/// Left is an Operand (can be identifier, string literal, number, boolean or placeholder).
/// </summary>
public sealed class ConditionNode : Node
{
    /// <summary>
    /// Left-hand operand
    /// </summary>
    public Operand Left { get; set; } = new Operand(OperandKind.String, "");

    /// <summary>
    /// The operator name (as parsed)
    /// </summary>
    public string Operator { get; set; } = "";

    /// <summary>
    /// The right-hand operand of the condition.
    /// </summary>
    public Operand Right { get; set; } = new Operand(OperandKind.String, "");
}

public sealed class BinaryNode : Node
{
    public string LogicOp { get; set; } = ""; // "AND" | "OR"
    public Node Left { get; set; } = default!;
    public Node Right { get; set; } = default!;
}

public sealed class NotNode : Node
{
    public Node Inner { get; set; } = default!;
}
