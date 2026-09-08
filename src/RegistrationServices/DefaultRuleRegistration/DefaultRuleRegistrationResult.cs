using RightRule.RuleModels;

namespace RightRule.RegistrationServices.DefaultRuleRegistration;

public class DefaultRuleRegistrationResult
{
    public RegistrationStatus Status { get; init; }
    public DefaultRule? RegisteredRule { get; init; }
    public string Message { get; init; }

    private DefaultRuleRegistrationResult(RegistrationStatus status, string message, DefaultRule? defaultRule)
    {
        Status = status;
        Message = message;
        RegisteredRule = defaultRule;
    }

    internal static DefaultRuleRegistrationResult Success(string message, DefaultRule defaultRule)
    {
        return new DefaultRuleRegistrationResult
            (
            RegistrationStatus.SUCCESS,
            message,
            defaultRule
            );
    }

    internal static DefaultRuleRegistrationResult Failed(string message)
    {
        return new DefaultRuleRegistrationResult
            (
            RegistrationStatus.FAILED,
            message,
            default
            );
    }

    internal static DefaultRuleRegistrationResult Failed(string message, DefaultRule defaultRule)
    {
        return new DefaultRuleRegistrationResult
            (
            RegistrationStatus.FAILED,
            message,
            defaultRule
            );
    }
}
