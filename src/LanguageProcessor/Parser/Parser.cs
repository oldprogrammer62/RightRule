using RightRule.LanguageProcessor.Tokenizer;

namespace RightRule.LanguageProcessor.Parser;

internal sealed class Parser : IParser
{
    private readonly string _statement = string.Empty;

    private readonly ITokenizer _tokenizer;
    private Token? _current;

    public Parser(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public Parser(ITokenizer tokenizer, string statement)
    {
        _tokenizer = tokenizer;
        _statement = statement;
    }

    public Node Parse()
    {
        _current = _tokenizer.NextToken();
        return ParseOr();
    }

    private void Eat(TokenTypeEnum TokenType)
    {
        if (_current is null)
            throw new Exception($"Expected {TokenType} but got no current token");

        if (_current.Type != TokenType)
            throw new Exception($"Expected {TokenType} but got {_current.Type} ('{_current.Value}') at pos {_current.Position}");

        _current = _tokenizer.NextToken();
    }

    private Node ContinueParse()
    {
        return ParseOr();
    }

    private Node ParseOr()
    {
        Node node = ParseAnd();

        if (_current is null)
            throw new Exception($"No current token");

        while (_current.Type == TokenTypeEnum.OR)
        {
            Eat(TokenTypeEnum.OR);
            node = new BinaryNode { LogicOp = "OR", Left = node, Right = ParseAnd() };
        }
        return node;
    }

    private Node ParseAnd()
    {
        Node node = ParseNot();

        if (_current is null)
            throw new Exception($"No current token");

        while (_current.Type == TokenTypeEnum.AND)
        {
            Eat(TokenTypeEnum.AND);
            node = new BinaryNode { LogicOp = "AND", Left = node, Right = ParseNot() };
        }
        return node;
    }

    private Node ParseNot()
    {
        if (_current is null)
            throw new Exception($"No current token");

        if (_current.Type == TokenTypeEnum.NOT)
        {
            Eat(TokenTypeEnum.NOT);
            return new NotNode { Inner = ParseNot() }; // right-associative
        }
        return ParsePrimary();
    }

    private Node ParsePrimary()
    {
        if (_current is null)
            throw new Exception($"No current token");

        if (_current.Type == TokenTypeEnum.LPAREN)
        {
            Eat(TokenTypeEnum.LPAREN);
            var node = ContinueParse();
            Eat(TokenTypeEnum.RPAREN);
            return node;
        }
        return ParseCondition();
    }

    private Node ParseCondition()
    {
        if (_current is null)
            throw new Exception($"No current token");

        // Left operand: allow Identifier or literal (String/Number/Boolean)
        Operand left = _current.Type switch
        {
            TokenTypeEnum.VARIABLE => new Operand(OperandKindEnum.VARIABLE, _current.Value),
            TokenTypeEnum.IDENTIFIER => new Operand(OperandKindEnum.IDENTIFIER, _current.Value),
            TokenTypeEnum.STRING => new Operand(OperandKindEnum.STRING, _current.Value),
            TokenTypeEnum.NUMBER => new Operand(OperandKindEnum.NUMBER, _current.Value),
            TokenTypeEnum.BOOLEAN => new Operand(OperandKindEnum.BOOLEAN, _current.Value),
            TokenTypeEnum.ARRAY => new Operand(OperandKindEnum.ARRAY, _current.Value),
            _ => throw new Exception($"Expected left-hand identifier or literal at pos {_current.Position}")
        };
        Eat(_current.Type);

        if (_current.Type != TokenTypeEnum.OPERATOR)
            throw new Exception($"Expected operator at pos {_current.Position}");
        string op = _current.Value; // keep original casing; resolve case-insensitively
        Eat(TokenTypeEnum.OPERATOR);

        Operand rhs = _current.Type switch
        {
            TokenTypeEnum.VARIABLE => new Operand(OperandKindEnum.VARIABLE, _current.Value),
            TokenTypeEnum.IDENTIFIER => new Operand(OperandKindEnum.IDENTIFIER, _current.Value),
            TokenTypeEnum.STRING => new Operand(OperandKindEnum.STRING, _current.Value),
            TokenTypeEnum.NUMBER => new Operand(OperandKindEnum.NUMBER, _current.Value),
            TokenTypeEnum.BOOLEAN => new Operand(OperandKindEnum.BOOLEAN, _current.Value),
            TokenTypeEnum.ARRAY => new Operand(OperandKindEnum.ARRAY, _current.Value),
            _ => throw new Exception($"Expected right-hand value at pos {_current.Position}")
        };
        Eat(_current.Type);

        return new ConditionNode
        {
            Left = left,
            Operator = op,
            Right = rhs
        };
    }
}
