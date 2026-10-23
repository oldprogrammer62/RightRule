using RightRule.DataServices.Data;
using RightRule.Models;

namespace RightRule.DataServices.DataRegistration;

/// <summary>
/// Result of registering Data with RightRule
/// </summary>
public sealed class DataRegistrationResult
{
    public required RightRuleStatusEnum Status { get; init; }
    public string Name { get; init; } = string.Empty;
    public string DataType { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;

    public static DataRegistrationResult SuccessResult(string DatasetName, string Message)
    {
        return new DataRegistrationResult()
        {
            Status = RightRuleStatusEnum.SUCCESS,
            Name = DatasetName,
            DataType = DataTypes.DATASET,
            Message = Message
        };
    }

    public static DataRegistrationResult FailureResult(string DatasetName, string Message)
    {
        return new DataRegistrationResult()
        {
            Status = RightRuleStatusEnum.FAILURE,
            Name = DatasetName,
            DataType = DataTypes.DATASET,
            Message = Message
        };
    }
}
