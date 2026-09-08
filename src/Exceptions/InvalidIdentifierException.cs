namespace RightRule.Exceptions;

public class InvalidIdentifierException : Exception
{
    public string Identifier { get; }
    public int Position { get; }

    public InvalidIdentifierException(string identifier, int position, string message, Exception innerException)
        : base($"Invalid identifier '{identifier}' at position {position}. {message}", innerException)
    {
        Identifier = identifier;
        Position = position;
    }

    public InvalidIdentifierException(string identifier, int position, string message)
        : base($"Invalid identifier '{identifier}' at position {position}. {message}")
    {
        Identifier = identifier;
        Position = position;
    }

    public InvalidIdentifierException(string identifier, int position)
        : base($"Invalid identifier '{identifier}' at position {position}. Identifiers must contain 1 dot and start with a letter or underscore and can contain letters, digits, underscores.")
    {
        Identifier = identifier;
        Position = position;
    }
}
