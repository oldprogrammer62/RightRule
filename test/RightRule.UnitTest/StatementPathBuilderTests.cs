using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.StatementBuilder;

namespace RightRuleTests;

public class StatementPathBuilderTests
{
    [Fact]
    public void BuildTreeText_NullNodeReturnsEmptyString()
    {
        Assert.Equal(string.Empty, StatementPathBuilder.BuildTreeText(null!));
    }

    [Theory]
    [InlineData(OperandKindEnum.VARIABLE, "threshold", "Variable(threshold)")]
    [InlineData(OperandKindEnum.IDENTIFIER, "Customer.Age", "Dataset.Identifier(Customer.Age)")]
    [InlineData(OperandKindEnum.NUMBER, "18", "Number(18)")]
    [InlineData(OperandKindEnum.STRING, "say \"hello\"", "String(\"say \\\"hello\\\"\")")]
    [InlineData(OperandKindEnum.BOOLEAN, "true", "Boolean(true)")]
    [InlineData(OperandKindEnum.ARRAY, "[1, 2]", "Operand([1, 2])")]
    [InlineData((OperandKindEnum)999, "raw", "Operand(raw)")]
    public void BuildTreeText_FormatsConditionOperands(
        OperandKindEnum leftKind,
        string leftRaw,
        string expectedLeft)
    {
        var node = Condition(
            leftKind,
            leftRaw,
            OperandKindEnum.NUMBER,
            "5",
            "equals");

        var result = StatementPathBuilder.BuildTreeText(node);

        Assert.Equal(TreeLine($"└─ Condition: {expectedLeft} equals Number(5)"), result);
    }

    [Fact]
    public void BuildTreeText_FormatsNestedBinaryAndNotNodes()
    {
        var node = new BinaryNode
        {
            LogicOp = "AND",
            Left = new NotNode
            {
                Inner = Condition(
                    OperandKindEnum.VARIABLE,
                    "enabled",
                    OperandKindEnum.BOOLEAN,
                    "false",
                    "equals")
            },
            Right = Condition(
                OperandKindEnum.IDENTIFIER,
                "Customer.Name",
                OperandKindEnum.STRING,
                "Alice",
                "equals")
        };

        var result = StatementPathBuilder.BuildTreeText(node);

        Assert.Equal(
            TreeLine(
                "└─ Binary: AND",
                "   ├─ NOT",
                "   │  └─ Condition: Variable(enabled) equals Boolean(false)",
                "   └─ Condition: Dataset.Identifier(Customer.Name) equals String(\"Alice\")"),
            result);
    }

    [Fact]
    public void BuildTreeText_FormatsUnknownNodeWithoutChildren()
    {
        var result = StatementPathBuilder.BuildTreeText(new UnknownNode());

        Assert.Equal(TreeLine("└─ Unknown node: UnknownNode"), result);
    }

    private static ConditionNode Condition(
        OperandKindEnum leftKind,
        string leftRaw,
        OperandKindEnum rightKind,
        string rightRaw,
        string operation) =>
        new()
        {
            Left = new Operand(leftKind, leftRaw),
            Operator = operation,
            Right = new Operand(rightKind, rightRaw)
        };

    private static string TreeLine(params string[] lines) =>
        string.Join(Environment.NewLine, lines) + Environment.NewLine;

    private sealed class UnknownNode : Node;
}