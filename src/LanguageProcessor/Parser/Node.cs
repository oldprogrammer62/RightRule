namespace RightRule.LanguageProcessor.Parser;

public abstract class Node { }

internal sealed class Operand
{
    public OperandKindEnum Kind { get; }
    public string Raw { get; }

    internal Operand(OperandKindEnum kind, string raw)
    {
        Kind = kind;
        Raw = raw;
    }
}

internal sealed class ConditionNode : Node
{
    // Left-hand operand (can be identifier, string literal, number, boolean)
    public Operand Left { get; set; } = new Operand(OperandKindEnum.STRING, "");

    // Operator name (as parsed)
    public string Operator { get; set; } = "";

    // Right-hand operand
    public Operand Right { get; set; } = new Operand(OperandKindEnum.STRING, "");
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
