using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleRegistration;

/// <summary>
/// Result of registering a Rule with RightRule
/// </summary>
public sealed class RuleRegistrationResult
{
    public required RegistrationStatusEnum Status { get; init; }
    public required ParsedRule? RegisteredRule { get; init; }
    public required string Message { get; init; }

    public static RuleRegistrationResult Success(ParsedRule ParsedRule, string Message)
    {
        return new RuleRegistrationResult()
        {
            Message = Message,
            RegisteredRule = ParsedRule,
            Status = ParsedRule.Status.ParserToRegistrationStatus()
        };
    }

    public static RuleRegistrationResult Failed(ParsedRule ParsedRule, string Message)
    {
        return new RuleRegistrationResult()
        {
            Message = Message,
            RegisteredRule = ParsedRule,
            Status = RegistrationStatusEnum.FAILURE
        };
    }

    internal static RuleRegistrationResult Failed(string Message)
    {
        return new RuleRegistrationResult()
        {
            Message = Message,
            RegisteredRule = null,
            Status = RegistrationStatusEnum.FAILURE
        };
    }
}
