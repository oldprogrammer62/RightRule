using RightRule.RuleModels.BaseModels;

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
    public required string Decision { get; init; }
    public object? DecisionData {  get; init; }

    public ServiceStatus Status { get; init; }
    public T? Payload { get; init; }
    public string Message { get; init; } = string.Empty;
    public Exception? Exception { get; init; }

    private RightRuleServiceResult() { }

    public static List<RightRuleServiceResult<IRule>> Success(List<IRule> payloads)
    {
        List<RightRuleServiceResult<IRule>> results = new();

        foreach (var payload in payloads)
        {
            var result = Success(payload);
            results.Add(result);
        }
        return results;
    }

    public static RightRuleServiceResult<IRule> Success(IRule? payload) =>
        new RightRuleServiceResult<IRule> { Status = ServiceStatus.Success, Message = string.Empty, Decision = payload.Decision, DecisionData = payload.DecisionData, Payload = payload };

    public static RightRuleServiceResult<IRule> Failure(string message, IRule? payload = default) =>
        new() { Status = ServiceStatus.Failure, Message = message, Decision = payload.Decision, DecisionData = payload.FalseDecisionData, Payload = payload };

    public static RightRuleServiceResult<IRule> Error(Exception ex, IRule? payload = default) =>
        new() { Status = ServiceStatus.Error, Message = ex.Message, Decision = payload.Decision, DecisionData = null, Payload = payload, Exception = ex };

    public static RightRuleServiceResult<IRule> Default(IRule? payload = default, string message = "Default") =>
        new() { Status = ServiceStatus.Default, Message = message, Payload = payload, Decision = payload.Decision, DecisionData = payload.DecisionData };
}
