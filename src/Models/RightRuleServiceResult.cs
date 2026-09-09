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
    public required bool IsSuccess { get; init; }
    public required bool Result { get; init; }

    public required string Decision { get; init; }
    public object? DecisionData {  get; init; }

    public required string StatementApplied { get; init; }

    public ServiceStatus Status { get; init; }
    public T? Payload { get; init; }
    public string Message { get; init; } = string.Empty;
    public Exception? Exception { get; init; }

    private RightRuleServiceResult() { }

    public static List<RightRuleServiceResult<IProcessedRule>> Success(List<IProcessedRule> payloads)
    {
        List<RightRuleServiceResult<IProcessedRule>> results = [];

        foreach (var payload in payloads)
        {
            var result = Success(payload);
            results.Add(result);
        }
        return results;
    }

    public static RightRuleServiceResult<IProcessedRule> Success(IProcessedRule payload)
    {
        return new RightRuleServiceResult<IProcessedRule> { Status = ServiceStatus.Success, IsSuccess = payload.IsSuccess, Result = payload.Result, Message = string.Empty, StatementApplied = payload.StatementApplied, Decision = payload.Decision, DecisionData = payload.DecisionData, Payload = payload };
    }

    public static RightRuleServiceResult<IProcessedRule> Build(IProcessedRule payload, string message)
    {
        return payload.Result switch
        {
            true => Success(payload),
            false => Failure(message, payload)
        };
    }

    public static RightRuleServiceResult<IProcessedRule> Failure(string message, IProcessedRule? payload = default)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Status = ServiceStatus.Failure, IsSuccess = payload.IsSuccess, Result = payload.Result, Message = message, StatementApplied = payload.StatementApplied, Decision = payload.FalseDecision, DecisionData = payload.FalseDecisionData, Payload = payload };
    }

    public static RightRuleServiceResult<IProcessedRule> Error(Exception ex, IProcessedRule? payload = default)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Status = ServiceStatus.Error, IsSuccess = payload.IsSuccess, Result = payload.Result, Message = ex.Message, StatementApplied = payload.StatementApplied, Decision = payload.Decision, DecisionData = null, Payload = payload, Exception = ex };
    }

    public static RightRuleServiceResult<IProcessedRule> Default(IProcessedRule? payload = default, string message = "Default")
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Status = ServiceStatus.Default, IsSuccess = payload.IsSuccess, Result = payload.Result, Message = message, StatementApplied = payload.StatementApplied, Payload = payload, Decision = payload.Decision, DecisionData = payload.DecisionData };
    }
}
