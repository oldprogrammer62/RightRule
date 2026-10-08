using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.DefaultRuleRegistration;

public class DefaultRuleRegistrationResult
{
    public RightRuleStatusEnum Status { get; init; }
    public DefaultRule? RegisteredRule { get; init; }
    public string Message { get; init; }

    private DefaultRuleRegistrationResult(RightRuleStatusEnum status, string message, DefaultRule? defaultRule)
    {
        Status = status;
        Message = message;
        RegisteredRule = defaultRule;
    }

    internal static DefaultRuleRegistrationResult Success(string message, DefaultRule defaultRule)
    {
        return new DefaultRuleRegistrationResult
            (
            RightRuleStatusEnum.SUCCESS,
            message,
            defaultRule
            );
    }

    internal static DefaultRuleRegistrationResult Failed(string message)
    {
        return new DefaultRuleRegistrationResult
            (
            RightRuleStatusEnum.FAILURE,
            message,
            default
            );
    }

    internal static DefaultRuleRegistrationResult Failed(string message, DefaultRule defaultRule)
    {
        return new DefaultRuleRegistrationResult
            (
            RightRuleStatusEnum.FAILURE,
            message,
            defaultRule
            );
    }
}
