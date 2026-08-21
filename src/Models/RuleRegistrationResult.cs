using RightRule.RuleModels;

namespace RightRule.Models;

public class RuleRegistrationResult
{
    public bool IsSuccess { get; init; }
    public ParsedRule RegisteredRule { get; init; }
    public string Message { get; init; }

    private RuleRegistrationResult(bool isSuccess, string message, ParsedRule parsedRule)
    {
        IsSuccess = isSuccess;
        Message = message;
        RegisteredRule = parsedRule;
    }

    internal static RuleRegistrationResult Success(string message, ParsedRule parsedRule)
    {
        return new RuleRegistrationResult(
            true,
            message,
            parsedRule
            );
    }

    internal static RuleRegistrationResult Failed(string message, ParsedRule parsedRule)
    {
        return new RuleRegistrationResult(
            false,
            message,
            parsedRule
            );
    }
}
