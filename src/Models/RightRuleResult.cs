using RightRule.RuleModels.BaseModels;

namespace RightRule.Models;

public enum RightRuleStatus
{
    Success,
    Failure,
    Error,
    Default
}

public sealed class RightRuleResult<T>
{
    public required string Decision { get; init; }
    public object? DecisionData { get; init; }

    public T? Payload { get; init; }
    public string Message { get; init; } = string.Empty;
    public Exception? Exception { get; init; }

    private RightRuleResult() { }

    public static RightRuleResult<IRule> Success(IRule? payload) =>
        new() { Decision = payload.Decision, DecisionData = payload.DecisionData, Payload = payload };

    public static RightRuleResult<IRule> Failure(string message, IRule? payload = default) =>
        new() { Message = message, Decision = payload.FalseDecision, DecisionData = payload?.FalseDecisionData, Payload = payload };

    public static RightRuleResult<IRule> Error(Exception ex, IRule? payload = default) =>
        new() { Message = ex.Message, Decision = payload.Decision, DecisionData = null, Payload = payload, Exception = ex};

    public static RightRuleResult<IRule> Default(IRule? payload = default, string message = "Default") =>
        new() { Message = message, Decision = payload.Decision, DecisionData = payload.DecisionData, Payload = payload };
}