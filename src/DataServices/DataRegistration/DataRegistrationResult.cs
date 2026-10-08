using RightRule.Models;

namespace RightRule.DataServices.DataRegistration;

/// <summary>
/// Result of registering Data with RightRule
/// </summary>
public class DataRegistrationResult
{
    public required RightRuleStatusEnum Status { get; init; }
    public string Name { get; init; } = string.Empty;
    public string DataType { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;

    internal static DataRegistrationResult SuccessResult(string name, string dataType, string message)
    {
        return new DataRegistrationResult()
        {
            Status = RightRuleStatusEnum.SUCCESS,
            Name = name,
            DataType = dataType,
            Message = message
        };
    }

    internal static DataRegistrationResult FailureResult(string name, string dataType, string message)
    {
        return new DataRegistrationResult()
        {
            Status = RightRuleStatusEnum.FAILURE,
            Name = name,
            DataType = dataType,
            Message = message
        };
    }
}
