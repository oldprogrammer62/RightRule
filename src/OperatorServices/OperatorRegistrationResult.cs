using RightRule.Models;

namespace RightRule.OperatorServices;

public sealed class OperatorRegistrationResult
{
    public required RegistrationStatusEnum Status { get; init; }
    public required string Message { get; init; }

    internal static OperatorRegistrationResult Success(string Message)
    {
        return new OperatorRegistrationResult()
        {
            Message = Message,
            Status = RegistrationStatusEnum.SUCCESS
        };
    }

    internal static OperatorRegistrationResult Failed(string Message)
    {
        return new OperatorRegistrationResult()
        {
            Message = Message,
            Status = RegistrationStatusEnum.FAILURE
        };
    }
}
