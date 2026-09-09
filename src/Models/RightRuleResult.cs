using RightRule.Models.BaseModels;
using System.Text;

namespace RightRule.Models;

public sealed class RightRuleResult<T>
{
    public required int RuleNumber { get; init; }
    public required string RuleName { get; init; }
    public required bool Result { get; init; }


    public required string Decision { get; init; }
    public object? DecisionData { get; init; }
    
    public required string StatementApplied { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    private RightRuleResult() { }

    public static RightRuleResult<IProcessedRule> SuccessResult(IProcessedRule? payload, string statementApplied)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Result = payload.Result, RuleNumber = payload.Number, RuleName = payload.Name, Decision = payload.Decision, DecisionData = payload.DecisionData, StatementApplied = statementApplied };
    }

    public static RightRuleResult<IProcessedRule> FailureResult(string message, string statementApplied, IProcessedRule? payload = default)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Result = payload.Result, RuleNumber = payload.Number, RuleName = payload.Name, Message = message, Decision = payload.FalseDecision, DecisionData = payload?.FalseDecisionData, StatementApplied = statementApplied };
    }

    public static RightRuleResult<IProcessedRule> ErrorResult(Exception ex, IProcessedRule? payload = default)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Result = payload.Result, RuleNumber = payload.Number, RuleName = payload.Name, Message = ex.Message, Decision = payload.Decision, DecisionData = null, StatementApplied = string.Empty };
    }

    public static RightRuleResult<IProcessedRule> DefaultResult(IProcessedRule? payload = default, string message = "Default")
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() { Result = payload.Result, RuleNumber = payload.Number, RuleName = payload.Name, Message = message, Decision = payload.Decision, DecisionData = payload.DecisionData, StatementApplied = string.Empty };
    }

    public new string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Number: {RuleNumber}");
        sb.AppendLine($"Name: {RuleName}");
        sb.AppendLine($"StatementApplied: {StatementApplied}");
        sb.AppendLine($"Decision: {Decision}");
        sb.AppendLine($"DecisionData: {DecisionData}");
        sb.AppendLine($"Result: {Result}");
        sb.AppendLine($"Message: {Message}");
        return sb.ToString();
    }
}