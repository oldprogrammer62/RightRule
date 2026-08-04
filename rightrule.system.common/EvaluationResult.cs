namespace RightRule.System.Common;

public class EvaluationResult
{
    public bool IsSuccess { get; }
    public bool Value { get; }
    public string Message { get; }

    private EvaluationResult(bool isSuccess, bool value, string message)
    {
        IsSuccess = isSuccess;
        Value = value;
        Message = message;
    }

    public EvaluationResult Invert() => new EvaluationResult(IsSuccess, !Value, Message);

    public static EvaluationResult Success(bool value) => new EvaluationResult(true, value, string.Empty);
    public static EvaluationResult Failure(string message) => new EvaluationResult(false, default, message);
}
