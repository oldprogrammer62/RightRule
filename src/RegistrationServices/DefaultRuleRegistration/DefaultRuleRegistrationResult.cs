using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.RuleRegistration;

namespace RightRule.RegistrationServices.DefaultRuleRegistration;

public class DefaultRuleRegistrationResult
{
    public RegistrationStatusEnum Status { get; init; }
    public DefaultRule? RegisteredRule { get; init; }
    public string Message { get; init; }

    private DefaultRuleRegistrationResult(RegistrationStatusEnum status, string message, DefaultRule? defaultRule)
    {
        Status = status;
        Message = message;
        RegisteredRule = defaultRule;
    }

    internal static DefaultRuleRegistrationResult Success(string message, DefaultRule defaultRule)
    {
        return new DefaultRuleRegistrationResult
            (
            RegistrationStatusEnum.SUCCESS,
            message,
            defaultRule
            );
    }

    internal static DefaultRuleRegistrationResult Failed(string message)
    {
        return new DefaultRuleRegistrationResult
            (
            RegistrationStatusEnum.FAILURE,
            message,
            default
            );
    }

    internal static DefaultRuleRegistrationResult Failed(string message, DefaultRule defaultRule)
    {
        return new DefaultRuleRegistrationResult
            (
            RegistrationStatusEnum.FAILURE,
            message,
            defaultRule
            );
    }
}
