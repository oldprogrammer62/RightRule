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
        : base($"Invalid identifier '{identifier}' at position {position}. Identifiers must start with a letter or underscore and can contain letters, digits, underscores. Identifiers can refer directly to a registered variable, or contain a single dot to refer to a data set and variable.")
    {
        Identifier = identifier;
        Position = position;
    }
}
