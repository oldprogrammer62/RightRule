namespace RightRule.StatementProcessor;

public abstract class Node { }

public enum OperandKind { Identifier, Number, String, Boolean }

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

public sealed class ConditionNode : Node
{
    // Left-hand operand (can be identifier, string literal, number, boolean or placeholder)
    public Operand Left { get; set; } = new Operand(OperandKind.String, "");

    // Operator name (as parsed)
    public string Operator { get; set; } = "";

    // Right-hand operand
    public Operand Right { get; set; } = new Operand(OperandKind.String, "");
}

public sealed class BinaryNode : Node
{
    public string LogicOp { get; set; } = ""; // "and" | "or"
    public Node Left { get; set; } = default!;
    public Node Right { get; set; } = default!;
}

public sealed class NotNode : Node
{
    public Node Inner { get; set; } = default!;
}
