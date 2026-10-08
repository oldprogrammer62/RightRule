using RightRule.Models;

namespace RightRule.DataProvider;

public class DataProviderRegistrationResult
{
    public required RightRuleStatusEnum Status { get; init; }
    public required string Message { get; init; }

    internal static DataProviderRegistrationResult Success(string Message)
    {
        return new DataProviderRegistrationResult()
        {
            Message = Message,
            Status = RightRuleStatusEnum.SUCCESS
        };
    }

    internal static DataProviderRegistrationResult Failed(string Message)
    {
        return new DataProviderRegistrationResult()
        {
            Message = Message,
            Status = RightRuleStatusEnum.FAILURE
        };
    }
}
