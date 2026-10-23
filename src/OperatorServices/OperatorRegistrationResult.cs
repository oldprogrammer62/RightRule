using RightRule.Models;

namespace RightRule.OperatorServices;

public sealed class OperatorRegistrationResult
{
    public required RightRuleStatusEnum Status { get; init; }
    public required string Message { get; init; }

    internal static OperatorRegistrationResult Success(string Message)
    {
        return new OperatorRegistrationResult()
        {
            Message = Message,
            Status = RightRuleStatusEnum.SUCCESS
        };
    }

    internal static OperatorRegistrationResult Failed(string Message)
    {
        return new OperatorRegistrationResult()
        {
            Message = Message,
            Status = RightRuleStatusEnum.FAILURE
        };
    }
}
