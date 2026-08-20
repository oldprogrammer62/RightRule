namespace RightRule.StatementProcessor.Tokenizer;

public sealed class Token
{
    internal TokenType Type { get; }
    internal string Value { get; }
    internal int Position { get; } // optional for better error messages

    internal Token(TokenType type, string value, int position = -1)
    {
        Type = type;
        Value = value;
        Position = position;
    }

    public override string ToString() => $"{Type}: {Value} @ {Position}";
}
