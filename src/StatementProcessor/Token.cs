namespace RightRule.StatementProcessor;

public sealed class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Position { get; } // optional for better error messages

    public Token(TokenType type, string value, int position = -1)
    {
        Type = type;
        Value = value;
        Position = position;
    }

    public override string ToString() => $"{Type}: {Value} @ {Position}";
}
