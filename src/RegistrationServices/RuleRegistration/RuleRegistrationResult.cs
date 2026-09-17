using RightRule.Models.RuleModels;

namespace RightRule.RegistrationServices.RuleRegistration;

/// <summary>
/// Result of registering a Rule with RightRule
/// </summary>
public class RuleRegistrationResult
{
    public required RegistrationStatusEnum Status { get; init; }
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
            Status = RegistrationStatusEnum.FAILURE
        };
    }

    internal static RuleRegistrationResult Failed(string message)
    {
        return new RuleRegistrationResult()
        {
            Message = message,
            RegisteredRule = null,
            Status = RegistrationStatusEnum.FAILURE
        };
    }
}
