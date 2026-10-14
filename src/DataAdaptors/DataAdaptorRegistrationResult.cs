using RightRule.Models;

namespace RightRule.DataAdaptors;

public class DataAdaptorRegistrationResult
{
    public required RightRuleStatusEnum Status { get; init; }
    public required string Message { get; init; }

    internal static DataAdaptorRegistrationResult Success(string Message)
    {
        return new DataAdaptorRegistrationResult()
        {
            Message = Message,
            Status = RightRuleStatusEnum.SUCCESS
        };
    }

    internal static DataAdaptorRegistrationResult Failed(string Message)
    {
        return new DataAdaptorRegistrationResult()
        {
            Message = Message,
            Status = RightRuleStatusEnum.FAILURE
        };
    }
}
