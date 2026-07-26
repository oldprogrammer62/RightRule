namespace RightRule.StatementProcessor;

public sealed class Parser
{
    private readonly Tokenizer _tokenizer;
    private Token _current;

    internal Parser(Tokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    internal async Task<Node> ParseAsync()
    {
        _current = await _tokenizer.NextTokenAsync().ConfigureAwait(false);
        return await ParseOrAsync().ConfigureAwait(false);
    }

    private async Task EatAsync(TokenType type)
    {
        if (_current.Type != type)
            throw new Exception($"Expected {type} but got {_current.Type} ('{_current.Value}') at pos {_current.Position}");
        _current = await _tokenizer.NextTokenAsync().ConfigureAwait(false);
    }

    private async Task<Node> ContinueParseAsync()
    {
        return await ParseOrAsync().ConfigureAwait(false);
    }

    private async Task<Node> ParseOrAsync()
    {
        Node node = await ParseAndAsync().ConfigureAwait(false);
        while (_current.Type == TokenType.Or)
        {
            await EatAsync(TokenType.Or).ConfigureAwait(false);
            node = new BinaryNode { LogicOp = "OR", Left = node, Right = await ParseAndAsync().ConfigureAwait(false) };
        }
        return node;
    }

    private async Task<Node> ParseAndAsync()
    {
        Node node = await ParseNotAsync().ConfigureAwait(false);
        while (_current.Type == TokenType.And)
        {
            await EatAsync(TokenType.And).ConfigureAwait(false);
            node = new BinaryNode { LogicOp = "AND", Left = node, Right = await ParseNotAsync().ConfigureAwait(false) };
        }
        return node;
    }

    private async Task<Node> ParseNotAsync()
    {
        if (_current.Type == TokenType.Not)
        {
            await EatAsync(TokenType.Not).ConfigureAwait(false);
            return new NotNode { Inner = await ParseNotAsync().ConfigureAwait(false) }; // right-associative
        }
        return await ParsePrimaryAsync().ConfigureAwait(false);
    }

    private async Task<Node> ParsePrimaryAsync()
    {
        if (_current.Type == TokenType.LParen)
        {
            await EatAsync(TokenType.LParen).ConfigureAwait(false);
            var node = await ContinueParseAsync().ConfigureAwait(false);
            await EatAsync(TokenType.RParen).ConfigureAwait(false);
            return node;
        }
        return await ParseConditionAsync().ConfigureAwait(false);
    }

    private async Task<Node> ParseConditionAsync()
    {
        // Left operand: allow Identifier or literal (String/Number/Boolean/PlaceHolder)
        Operand left = _current.Type switch
        {
            TokenType.Identifier => new Operand(OperandKind.Identifier, _current.Value),
            TokenType.String => new Operand(OperandKind.String, _current.Value),
            TokenType.Number => new Operand(OperandKind.Number, _current.Value),
            TokenType.Boolean => new Operand(OperandKind.Boolean, _current.Value),
            _ => throw new Exception($"Expected left-hand identifier or literal at pos {_current.Position}")
        };
        await EatAsync(_current.Type).ConfigureAwait(false);

        if (_current.Type != TokenType.Operator)
            throw new Exception($"Expected operator at pos {_current.Position}");
        string op = _current.Value; // keep original casing; resolve case-insensitively
        await EatAsync(TokenType.Operator).ConfigureAwait(false);

        Operand rhs = _current.Type switch
        {
            TokenType.Identifier => new Operand(OperandKind.Identifier, _current.Value),
            TokenType.String => new Operand(OperandKind.String, _current.Value),
            TokenType.Number => new Operand(OperandKind.Number, _current.Value),
            TokenType.Boolean => new Operand(OperandKind.Boolean, _current.Value),
            _ => throw new Exception($"Expected right-hand value at pos {_current.Position}")
        };
        await EatAsync(_current.Type).ConfigureAwait(false);

        return new ConditionNode
        {
            Left = left,
            Operator = op,
            Right = rhs
        };
    }
}
