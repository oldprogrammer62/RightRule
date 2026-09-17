using RightRule.LanguageProcessor.Tokenizer;

namespace RightRule.LanguageProcessor.Parser;

internal sealed class Parser : IParser
{
    public bool DataCheck { get; set; } = false;
    public string Statement { get; set; } = string.Empty;

    private readonly ITokenizer _tokenizer;
    private Token? _current;

    public Parser(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public Node Parse()
    {
        _tokenizer.Statement = this.Statement;
        _tokenizer.Pos = 0;

        _current = _tokenizer.NextToken(DataCheck);
        return ParseOr();
    }

    private void Eat(TokenType type)
    {
        if (_current is null)
            throw new Exception($"Expected {type} but got no current token");

        if (_current.Type != type)
            throw new Exception($"Expected {type} but got {_current.Type} ('{_current.Value}') at pos {_current.Position}");

        _current = _tokenizer.NextToken(DataCheck);
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

        while (_current.Type == TokenType.Or)
        {
            Eat(TokenType.Or);
            node = new BinaryNode { LogicOp = "OR", Left = node, Right = ParseAnd() };
        }
        return node;
    }

    private Node ParseAnd()
    {
        Node node = ParseNot();

        if (_current is null)
            throw new Exception($"No current token");

        while (_current.Type == TokenType.And)
        {
            Eat(TokenType.And);
            node = new BinaryNode { LogicOp = "AND", Left = node, Right = ParseNot() };
        }
        return node;
    }

    private Node ParseNot()
    {
        if (_current is null)
            throw new Exception($"No current token");

        if (_current.Type == TokenType.Not)
        {
            Eat(TokenType.Not);
            return new NotNode { Inner = ParseNot() }; // right-associative
        }
        return ParsePrimary();
    }

    private Node ParsePrimary()
    {
        if (_current is null)
            throw new Exception($"No current token");

        if (_current.Type == TokenType.LParen)
        {
            Eat(TokenType.LParen);
            var node = ContinueParse();
            Eat(TokenType.RParen);
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
            TokenType.Variable => new Operand(OperandKind.Variable, _current.Value),
            TokenType.Identifier => new Operand(OperandKind.Identifier, _current.Value),
            TokenType.String => new Operand(OperandKind.String, _current.Value),
            TokenType.Number => new Operand(OperandKind.Number, _current.Value),
            TokenType.Boolean => new Operand(OperandKind.Boolean, _current.Value),
            _ => throw new Exception($"Expected left-hand identifier or literal at pos {_current.Position}")
        };
        Eat(_current.Type);

        if (_current.Type != TokenType.Operator)
            throw new Exception($"Expected operator at pos {_current.Position}");
        string op = _current.Value; // keep original casing; resolve case-insensitively
        Eat(TokenType.Operator);

        Operand rhs = _current.Type switch
        {
            TokenType.Variable => new Operand(OperandKind.Variable, _current.Value),
            TokenType.Identifier => new Operand(OperandKind.Identifier, _current.Value),
            TokenType.String => new Operand(OperandKind.String, _current.Value),
            TokenType.Number => new Operand(OperandKind.Number, _current.Value),
            TokenType.Boolean => new Operand(OperandKind.Boolean, _current.Value),
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
