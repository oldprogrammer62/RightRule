using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration;

public class RuleRegistrationResult
{
    public required RegistrationStatus Status { get; init; }
    public required ParsedRule? RegisteredRule { get; init; }
    public required string Message { get; init; }

    internal static RuleRegistrationResult Success(string message, ParsedRule parsedRule)
    {
        return new RuleRegistrationResult()
        {
            Message = message,
            RegisteredRule = parsedRule,
            Status = parsedRule.Status
        };
    }

    internal static RuleRegistrationResult Failed(string message, ParsedRule parsedRule)
    {
        return new RuleRegistrationResult()
        {
            Message = message,
            RegisteredRule = parsedRule,
            Status = RegistrationStatus.FAILURE
        };
    }

    internal static RuleRegistrationResult Failed(string message)
    {
        return new RuleRegistrationResult()
        {
            Message = message,
            RegisteredRule = null,
            Status = RegistrationStatus.FAILURE
        };
    }
}
