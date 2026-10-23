namespace RightRule.Exceptions;

public class NoRulesRegisteredException : Exception
{
    public NoRulesRegisteredException() : base("No Rules Registered!")
    {
    }

    public NoRulesRegisteredException(string? message) : base(message)
    {
    }

    public NoRulesRegisteredException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
