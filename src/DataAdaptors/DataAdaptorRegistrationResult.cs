using RightRule.Models;

namespace RightRule.DataAdaptors;

public sealed class DataAdaptorRegistrationResult
{
    public required RegistrationStatusEnum Status { get; init; }
    public required string Message { get; init; }

    internal static DataAdaptorRegistrationResult Success(string Message)
    {
        return new DataAdaptorRegistrationResult()
        {
            Message = Message,
            Status = RegistrationStatusEnum.SUCCESS
        };
    }

    internal static DataAdaptorRegistrationResult Failed(string Message)
    {
        return new DataAdaptorRegistrationResult()
        {
            Message = Message,
            Status = RegistrationStatusEnum.FAILURE
        };
    }
}
