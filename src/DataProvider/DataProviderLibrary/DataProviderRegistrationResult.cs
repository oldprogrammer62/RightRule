using RightRule.Models;

namespace RightRule.DataProvider.DataProviderLibrary;

public class DataProviderRegistrationResult
{
    public required RightRuleStatusEnum Status { get; init; }
    public required string Message { get; init; }

    internal DataProviderRegistrationResult Success(string Message)
    {
        return new DataProviderRegistrationResult()
        {
            Message = Message,
            Status = RightRuleStatusEnum.SUCCESS
        };
    }

    internal DataProviderRegistrationResult Failed(string Message)
    {
        return new DataProviderRegistrationResult()
        {
            Message = Message,
            Status = RightRuleStatusEnum.FAILURE
        };
    }
}
