namespace RightRule.RegistrationServices.DataRegistration;

public class DataRegistrationResult
{
    public bool IsSuccess { get; init; }
    public string Name { get; init; }
    public string DataType { get; init; }
    public string Message { get; init; }

    private DataRegistrationResult(bool isSuccess, string name, string dataType, string message)
    {
        IsSuccess = isSuccess;
        Name = name;
        DataType = dataType;
        Message = message;
    }

    internal static DataRegistrationResult Success(string name, string dataType, string message)
    {
        return new DataRegistrationResult(
            true,
            name,
            dataType,
            message);
    }

    internal static DataRegistrationResult Failed(string name, string dataType, string message)
    {
        return new DataRegistrationResult(
            false,
            name,
            dataType,
            message);
    }
}
