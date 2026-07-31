namespace RightRule.StatementProcessor;

public abstract class Node { }

internal enum OperandKind { Identifier, Number, String, Boolean }

internal sealed class Operand
{
    public OperandKind Kind { get; }
    public string Raw { get; }

    internal Operand(OperandKind kind, string raw)
    {
        Kind = kind;
        Raw = raw;
    }
}

internal sealed class ConditionNode : Node
{
    // Left-hand operand (can be identifier, string literal, number, boolean or placeholder)
    public Operand Left { get; set; } = new Operand(OperandKind.String, "");

    // Operator name (as parsed)
    public string Operator { get; set; } = "";

    // Right-hand operand
    public Operand Right { get; set; } = new Operand(OperandKind.String, "");
}

internal sealed class BinaryNode : Node
{
    public string LogicOp { get; set; } = ""; // "and" | "or"
    public Node Left { get; set; } = default!;
    public Node Right { get; set; } = default!;
}

internal sealed class NotNode : Node
{
    public Node Inner { get; set; } = default!;
}
