using RightRule.Models.BaseModels;

namespace RightRule.Models;

internal enum ServiceStatus
{
    Success,
    Failure,
    Error,
    Default
}

internal sealed class RightRuleServiceResult<T>
{
    public required bool Success { get; init; }
    public required bool Result { get; init; }

    public required string Decision { get; init; }
    public object? DecisionData {  get; init; }

    public required string StatementApplied { get; init; }

    public ServiceStatus Status { get; init; }
    public T? Payload { get; init; }
    public string Message { get; init; } = string.Empty;
    public Exception? Exception { get; init; }

    private RightRuleServiceResult() { }

    public static List<RightRuleServiceResult<IProcessedRule>> SuccessResult(List<IProcessedRule> payloads)
    {
        List<RightRuleServiceResult<IProcessedRule>> results = [];

        foreach (var payload in payloads)
        {
            var result = SuccessResult(payload);
            results.Add(result);
        }
        return results;
    }

    public static RightRuleServiceResult<IProcessedRule> SuccessResult(IProcessedRule payload)
    {
        return new RightRuleServiceResult<IProcessedRule> { Status = ServiceStatus.Success, Success = payload.Success, Result = payload.Result, Message = string.Empty, StatementApplied = payload.StatementApplied, Decision = payload.Decision, DecisionData = payload.DecisionData, Payload = payload };
    }

    public static RightRuleServiceResult<IProcessedRule> Build(IProcessedRule payload, string message)
    {
        return payload.Result switch
        {
            true => SuccessResult(payload),
            false => FailureResult(message, payload)
        };
    }

    public static RightRuleServiceResult<IProcessedRule> FailureResult(string message, IProcessedRule? payload = default)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Status = ServiceStatus.Failure, Success = payload.Success, Result = payload.Result, Message = message, StatementApplied = payload.StatementApplied, Decision = payload.FalseDecision, DecisionData = payload.FalseDecisionData, Payload = payload };
    }

    public static RightRuleServiceResult<IProcessedRule> ErrorResult(Exception ex, IProcessedRule? payload = default)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Status = ServiceStatus.Error, Success = payload.Success, Result = payload.Result, Message = ex.Message, StatementApplied = payload.StatementApplied, Decision = payload.Decision, DecisionData = null, Payload = payload, Exception = ex };
    }

    public static RightRuleServiceResult<IProcessedRule> DefaultResult(IProcessedRule? payload = default, string message = "Default")
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Status = ServiceStatus.Default, Success = payload.Success, Result = payload.Result, Message = message, StatementApplied = payload.StatementApplied, Payload = payload, Decision = payload.Decision, DecisionData = payload.DecisionData };
    }
}
