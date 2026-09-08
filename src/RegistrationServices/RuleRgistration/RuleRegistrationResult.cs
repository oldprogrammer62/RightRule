using RightRule.RuleModels.RuleModels;

namespace RightRule.RegistrationServices.RuleRgistration;

public class RuleRegistrationResult
{
    public required RegistrationStatus Status { get; init; }
    public required ParsedRule RegisteredRule { get; init; }
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
            Status = RegistrationStatus.FAILED
        };
    }
}
