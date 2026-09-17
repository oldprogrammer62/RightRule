using RightRule.RegistrationServices.RuleRegistration;

namespace RightRule.RegistrationServices.DataRegistration;

/// <summary>
/// Result of registering Data with RightRule
/// </summary>
public class DataRegistrationResult
{
    public required RegistrationStatusEnum Status { get; init; }
    public string Name { get; init; } = string.Empty;
    public string DataType { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;

    internal static DataRegistrationResult SuccessResult(string name, string dataType, string message)
    {
        return new DataRegistrationResult()
        {
            Status = RegistrationStatusEnum.SUCCESS,
            Name = name,
            DataType = dataType,
            Message = message
        };
    }

    internal static DataRegistrationResult FailureResult(string name, string dataType, string message)
    {
        return new DataRegistrationResult()
        {
            Status = RegistrationStatusEnum.FAILURE,
            Name = name,
            DataType = dataType,
            Message = message
        };
    }
}
