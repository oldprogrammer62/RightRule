namespace RightRule.System.Common;

public record Error(string Code, string Message);

public record xOpResult(bool IsSuccess, Error? Error)
{
    public bool IsFailure => !IsSuccess;
    public static xOpResult Success() => new(true, null);
    public static xOpResult Failure(Error error) => new(false, error);
}

// Generic Result to hold a value on success
public record xOpResult<T>(T? Value, Error? Error, bool IsSuccess) : xOpResult(IsSuccess, Error)
{
    public static xOpResult<T> Success(T value) => new(value, null, true);
    public static new xOpResult<T> Failure(Error error) => new(default, error, false);

    // Implicit conversions to make return statements cleaner
    public static implicit operator xOpResult<T>(T value) => Success(value);
    public static implicit operator xOpResult<T>(Error error) => Failure(error);
}
