using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleRegistration;

/// <summary>
/// Result of registering a Rule with RightRule
/// </summary>
public class RuleRegistrationResult
{
    public required RightRuleStatusEnum Status { get; init; }
    public required ParsedRule? RegisteredRule { get; init; }
    public required string Message { get; init; }

    internal static RuleRegistrationResult Success(string Message, ParsedRule ParsedRule)
    {
        return new RuleRegistrationResult()
        {
            Message = Message,
            RegisteredRule = ParsedRule,
            Status = ParsedRule.Status
        };
    }

    internal static RuleRegistrationResult Failed(string Message, ParsedRule ParsedRule)
    {
        return new RuleRegistrationResult()
        {
            Message = Message,
            RegisteredRule = ParsedRule,
            Status = RightRuleStatusEnum.FAILURE
        };
    }

    internal static RuleRegistrationResult Failed(string Message)
    {
        return new RuleRegistrationResult()
        {
            Message = Message,
            RegisteredRule = null,
            Status = RightRuleStatusEnum.FAILURE
        };
    }
}
