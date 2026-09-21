namespace RightRule.LanguageProcessor.Tokenizer;

public sealed class Token
{
    internal TokenTypeEnum Type { get; }
    internal string Value { get; }
    internal int Position { get; } // optional for better error messages

    internal Token(TokenTypeEnum TokenType, string Value, int Position = -1)
    {
        Type = TokenType;
        this.Value = Value;
        this.Position = Position;
    }

    public override string ToString() => $"{Type}: {Value} @ {Position}";
}
