namespace RightRule.Interpreter;

/// <summary>
/// Node is the base class for all nodes in the abstract syntax tree (AST)
/// A Node represents the parsed rule statement.
/// It is an abstract class that cannot be instantiated directly.
/// It serves as a common base for all types of nodes, such as condition nodes, binary nodes, and not nodes. Each node type has its own properties and methods that are specific to its functionality, but they all inherit from the Node class, allowing them to be treated polymorphically when traversing the AST during evaluation.
/// </summary>
public abstract class Node { }

/// <summary>
/// OperandKind is an enumeration that represents the different types of operands that can be used in a condition node.
/// It includes Identifier, Number, String, Boolean, and Variable.
/// This allows the interpreter to understand the type of operand being used and evaluate it accordingly during the evaluation of the rule statement.
/// <list type="bullet">
///    <listheader>
///        <term>Identifier</term>
///        <description>Represents the name of a data item in the Dictionary of data</description>
///    </listheader>
///    <item>
///        <term>Number</term>
///        <description>A numeric value</description>
///    </item>
///    <item>
///        <term>String</term>
///        <description>A string value</description>
///    </item>
///    <item>
///        <term>Boolean</term>
///        <description>A boolean value (true or false)</description>
///    </item>
///    <item>
///        <term>Variable</term>
///        <description>Represents a replaceable placeholder - identified as {name}</description>
///    </item>
/// </list>
/// </summary>
public enum OperandKind { Identifier, Number, String, Boolean, Placeholder }

/// <summary>
/// Provides a way to represent the right-hand side operand of a condition in a structured way.
/// It encapsulates the type of the operand (such as number, string, boolean, identifier, or placeholder) and its raw string value as parsed from the rule statement.
/// This allows the interpreter to understand the type of operand being used and evaluate it accordingly during the evaluation of the rule statement.
/// </summary>
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
/// Represents a simple condition in the rule statement, such as "numberoforders > 10" or "country == 'UK'".
/// </summary>
public sealed class ConditionNode : Node
{
    /// <summary>
    /// The name of the data item in the Dictionary of data to be evaluated. It is the left-hand side of a condition. For example, in the condition "numberoforders > 10", "numberoforders" is the LeftIdentifier. The interpreter will look up the value of "numberoforders" in the provided data during evaluation and compare it to the right-hand side of the condition using the specified operator.
    /// </summary>
    public string LeftIdentifier { get; set; } = ""; // variable name

    /// <summary>
    /// The logic operator used to compare the left-hand side identifier to the right-hand side operand. For example, in the condition "numberoforders > 10", ">" is the Operator. The interpreter will use this operator to compare the value of "numberoforders" from the data to the operand "10" during evaluation. The operator is stored as a string as parsed, and it will be resolved case-insensitively during evaluation to determine the actual operation to perform.
    /// </summary>
    public string Operator { get; set; } = "";       // operator name (as parsed)

    /// <summary>
    /// The right-hand side operand of a condition. For example, in the condition "numberoforders > 10", "10" is the Right operand. The Operand class encapsulates the type of the operand (such as number, string, boolean, identifier, or variable) and its raw string value as parsed from the rule statement. During evaluation, the interpreter will resolve this operand to its actual value based on its type and compare it to the left-hand side identifier using the specified operator.
    /// </summary>
    public Operand Right { get; set; } = new Operand(OperandKind.String, "");
}

/// <summary>
/// Represents a binary expression in the rule statement, such as "numberoforders > 10 AND country == 'UK'".
/// </summary>
public sealed class BinaryNode : Node
{
    /// <summary>
    /// The logical operator that connects the left and right nodes in a binary expression. It can be either "AND" or "OR". For example, in the expression "numberoforders > 10 AND country == 'USA'", "AND" is the LogicOp that connects the two conditions. The interpreter will use this operator to determine how to combine the results of evaluating the left and right nodes during evaluation. The operator is stored as a string as parsed, and it will be resolved case-insensitively during evaluation to determine whether to perform a logical AND or OR operation on the results of the left and right nodes.
    /// </summary>
    public string LogicOp { get; set; } = ""; // "AND" | "OR"

    /// <summary>
    /// Represents the left-hand side of a binary expression. For example, in the expression "numberoforders > 10 AND country == 'USA'", the condition "numberoforders > 10" would be represented as the Left node. The Left node can be any type of Node, such as a ConditionNode, another BinaryNode, or a NotNode. During evaluation, the interpreter will evaluate the Left node and combine its result with the result of evaluating the Right node using the specified LogicOp.
    /// </summary>
    public Node Left { get; set; } = default!;

    /// <summary>
    /// Represents the right-hand side of a binary expression. For example, in the expression "numberoforders > 10 AND country == 'USA'", the condition "country == 'USA'" would be represented as the Right node. The Right node can be any type of Node, such as a ConditionNode, another BinaryNode, or a NotNode. During evaluation, the interpreter will evaluate the Right node and combine its result with the result of evaluating the Left node using the specified LogicOp.
    /// </summary>
    public Node Right { get; set; } = default!;
}

/// <summary>
/// Represents a NOT expression in the rule statement, such as "NOT (numberoforders > 10)".
/// </summary>
public sealed class NotNode : Node
{
    /// <summary>
    /// Represents the inner node of a NOT expression. For example, in the expression "NOT (numberoforders > 10)", the condition "numberoforders > 10" would be represented as the Inner node. The Inner node can be any type of Node, such as a ConditionNode, a BinaryNode, or another NotNode. During evaluation, the interpreter will evaluate the Inner node and negate its result.
    /// </summary>
    public Node Inner { get; set; } = default!;
}
