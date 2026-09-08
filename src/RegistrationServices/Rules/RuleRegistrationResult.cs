using RightRule.RuleModels;

namespace RightRule.RegistrationServices.Rules;

public class RuleRegistrationResult
{
    public RegistrationStatus Status { get; init; }

    public bool IsSuccess { get; init; }

    public ParsedRule RegisteredRule { get; init; }
    public string Message { get; init; }

    private RuleRegistrationResult(bool isSuccess, string message, ParsedRule parsedRule)
    {
        Status = parsedRule.Status;
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
