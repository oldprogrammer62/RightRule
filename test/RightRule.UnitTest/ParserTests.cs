using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.Tokenizer;

namespace RightRuleTests;

public class ParserTests
{
    [Fact]
    public void Parse_ConditionPreservesOperandsAndOperator()
    {
        var result = Parse(
            Token(TokenTypeEnum.IDENTIFIER, "Customer.Age"),
            Token(TokenTypeEnum.OPERATOR, "GreaterThan"),
            Token(TokenTypeEnum.NUMBER, "18"));

        var condition = Assert.IsType<ConditionNode>(result);
        Assert.Equal(OperandKindEnum.IDENTIFIER, condition.Left.Kind);
        Assert.Equal("Customer.Age", condition.Left.Raw);
        Assert.Equal("GreaterThan", condition.Operator);
        Assert.Equal(OperandKindEnum.NUMBER, condition.Right.Kind);
        Assert.Equal("18", condition.Right.Raw);
    }

    [Fact]
    public void Parse_AndBindsMoreTightlyThanOr()
    {
        var result = Parse(
            ConditionTokens("A", "B")[0],
            ConditionTokens("A", "B")[1],
            ConditionTokens("A", "B")[2],
            Token(TokenTypeEnum.OR, "OR"),
            ConditionTokens("C", "D")[0],
            ConditionTokens("C", "D")[1],
            ConditionTokens("C", "D")[2],
            Token(TokenTypeEnum.AND, "AND"),
            ConditionTokens("E", "F")[0],
            ConditionTokens("E", "F")[1],
            ConditionTokens("E", "F")[2]);

        var or = Assert.IsType<BinaryNode>(result);
        Assert.Equal("OR", or.LogicOp);
        Assert.IsType<ConditionNode>(or.Left);

        var and = Assert.IsType<BinaryNode>(or.Right);
        Assert.Equal("AND", and.LogicOp);
        Assert.IsType<ConditionNode>(and.Left);
        Assert.IsType<ConditionNode>(and.Right);
    }

    [Fact]
    public void Parse_NotIsRightAssociative()
    {
        var result = Parse(
            Token(TokenTypeEnum.NOT, "NOT"),
            Token(TokenTypeEnum.NOT, "NOT"),
            Token(TokenTypeEnum.IDENTIFIER, "A"),
            Token(TokenTypeEnum.OPERATOR, "equals"),
            Token(TokenTypeEnum.IDENTIFIER, "B"));

        var outerNot = Assert.IsType<NotNode>(result);
        var innerNot = Assert.IsType<NotNode>(outerNot.Inner);
        Assert.IsType<ConditionNode>(innerNot.Inner);
    }

    [Fact]
    public void Parse_ParenthesesOverrideOperatorPrecedence()
    {
        var result = Parse(
            Token(TokenTypeEnum.LPAREN, "("),
            Token(TokenTypeEnum.IDENTIFIER, "A"),
            Token(TokenTypeEnum.OPERATOR, "equals"),
            Token(TokenTypeEnum.IDENTIFIER, "B"),
            Token(TokenTypeEnum.OR, "OR"),
            Token(TokenTypeEnum.IDENTIFIER, "C"),
            Token(TokenTypeEnum.OPERATOR, "equals"),
            Token(TokenTypeEnum.IDENTIFIER, "D"),
            Token(TokenTypeEnum.RPAREN, ")"),
            Token(TokenTypeEnum.AND, "AND"),
            Token(TokenTypeEnum.IDENTIFIER, "E"),
            Token(TokenTypeEnum.OPERATOR, "equals"),
            Token(TokenTypeEnum.IDENTIFIER, "F"));

        var and = Assert.IsType<BinaryNode>(result);
        Assert.Equal("AND", and.LogicOp);

        var or = Assert.IsType<BinaryNode>(and.Left);
        Assert.Equal("OR", or.LogicOp);
        Assert.IsType<ConditionNode>(and.Right);
    }

    [Fact]
    public void Parse_AcceptsEachSupportedOperandKindOnBothSides()
    {
        var operands = new[]
        {
            (TokenTypeEnum.VARIABLE, OperandKindEnum.VARIABLE),
            (TokenTypeEnum.IDENTIFIER, OperandKindEnum.IDENTIFIER),
            (TokenTypeEnum.STRING, OperandKindEnum.STRING),
            (TokenTypeEnum.NUMBER, OperandKindEnum.NUMBER),
            (TokenTypeEnum.BOOLEAN, OperandKindEnum.BOOLEAN),
            (TokenTypeEnum.ARRAY, OperandKindEnum.ARRAY)
        };

        foreach (var (tokenType, operandKind) in operands)
        {
            var result = Parse(
                Token(tokenType, "value"),
                Token(TokenTypeEnum.OPERATOR, "equals"),
                Token(tokenType, "value"));

            var condition = Assert.IsType<ConditionNode>(result);
            Assert.Equal(operandKind, condition.Left.Kind);
            Assert.Equal(operandKind, condition.Right.Kind);
        }
    }

    [Fact]
    public void Parse_RejectsUnsupportedLeftOperand()
    {
        var exception = Assert.Throws<Exception>(() =>
            Parse(Token(TokenTypeEnum.AND, "AND")));

        Assert.Contains("Expected left-hand identifier or literal", exception.Message);
    }

    [Fact]
    public void Parse_RejectsUnsupportedRightOperand()
    {
        var exception = Assert.Throws<Exception>(() =>
            Parse(
                Token(TokenTypeEnum.IDENTIFIER, "A"),
                Token(TokenTypeEnum.OPERATOR, "equals"),
                Token(TokenTypeEnum.AND, "AND")));

        Assert.Contains("Expected right-hand value", exception.Message);
    }

    [Fact]
    public void Parse_RejectsMissingOperator()
    {
        var exception = Assert.Throws<Exception>(() =>
            Parse(
                Token(TokenTypeEnum.IDENTIFIER, "A"),
                Token(TokenTypeEnum.IDENTIFIER, "B")));

        Assert.Contains("Expected operator", exception.Message);
    }

    [Fact]
    public void Parse_RejectsMissingClosingParenthesis()
    {
        var exception = Assert.Throws<Exception>(() =>
            Parse(
                Token(TokenTypeEnum.LPAREN, "("),
                Token(TokenTypeEnum.IDENTIFIER, "A"),
                Token(TokenTypeEnum.OPERATOR, "equals"),
                Token(TokenTypeEnum.IDENTIFIER, "B")));

        Assert.Contains("Expected RPAREN", exception.Message);
    }

    private static Node Parse(params Token[] tokens)
    {
        var remaining = new Queue<Token>(tokens);
        var tokenizer = new StubTokenizer(() =>
            remaining.Count > 0
                ? remaining.Dequeue()
                : Token(TokenTypeEnum.END, string.Empty));

        return new Parser(tokenizer).Parse();
    }

    private static Token[] ConditionTokens(string left, string right) =>
    [
        Token(TokenTypeEnum.IDENTIFIER, left),
        Token(TokenTypeEnum.OPERATOR, "equals"),
        Token(TokenTypeEnum.IDENTIFIER, right)
    ];

    private static Token Token(TokenTypeEnum type, string value) =>
        new(type, value);

    private sealed class StubTokenizer(Func<Token> nextToken) : ITokenizer
    {
        public Token NextToken() => nextToken();
    }
}