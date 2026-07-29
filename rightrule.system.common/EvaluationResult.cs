namespace RightRule.System.Common;

public class EvaluationResult
{
    public bool IsSuccess { get; }
    public bool Value { get; }
    public string ErrorMessage { get; }

    private EvaluationResult(bool isSuccess, bool value, string errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public EvaluationResult Invert() => new EvaluationResult(IsSuccess, !Value, ErrorMessage);

    public static EvaluationResult Success(bool value) => new EvaluationResult(true, value, string.Empty);
    public static EvaluationResult Failure(string error) => new EvaluationResult(false, default, error);
}
