namespace RightRule.System.Common.Exceptions;

public class SyntaxErrorException : Exception
{
    public string Statement { get; }

    public SyntaxErrorException()
    {
        Statement = string.Empty;
    }

    public SyntaxErrorException(string statement, string message) : base(message) 
    {
        Statement = statement;
    }

    public SyntaxErrorException(string statement, string message, Exception innerException) : base(message, innerException)
    {
        Statement = statement;
    }
}
