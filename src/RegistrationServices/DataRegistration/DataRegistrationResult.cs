namespace RightRule.RegistrationServices.DataRegistration;

public class DataRegistrationResult
{
    public bool Success { get; init; }
    public string Name { get; init; }
    public string DataType { get; init; }
    public string Message { get; init; }

    private DataRegistrationResult(bool Success, string name, string dataType, string message)
    {
        this.Success = Success;
        Name = name;
        DataType = dataType;
        Message = message;
    }

    internal static DataRegistrationResult SuccessResult(string name, string dataType, string message)
    {
        return new DataRegistrationResult(
            true,
            name,
            dataType,
            message);
    }

    internal static DataRegistrationResult FailureResult(string name, string dataType, string message)
    {
        return new DataRegistrationResult(
            false,
            name,
            dataType,
            message);
    }
}
