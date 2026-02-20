namespace RightRule.Interpreter;

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
    public string LeftIdentifier { get; set; } = ""; // variable name
    public string Operator { get; set; } = "";       // operator name (as parsed)
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
