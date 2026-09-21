using RightRule.Models.BaseModels;
using System.Text;

namespace RightRule;

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

    public static RightRuleResult<IProcessedRule> SuccessResult(IProcessedRule? Payload, string StatementApplied)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Result = Payload.Result, 
            RuleNumber = Payload.Number, 
            RuleName = Payload.Name, 
            Decision = Payload.Decision, 
            DecisionData = Payload.DecisionData, 
            StatementApplied = StatementApplied 
        };
    }

    public static RightRuleResult<IProcessedRule> FailureResult(string Message, string StatementApplied, IProcessedRule? Payload = default)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Result = Payload.Result, 
            RuleNumber = Payload.Number, 
            RuleName = Payload.Name, 
            Decision = Payload.FalseDecision, 
            DecisionData = Payload?.FalseDecisionData, 
            StatementApplied = StatementApplied,
            Message = Message 
        };
    }

    public static RightRuleResult<IProcessedRule> ErrorResult(Exception ex, IProcessedRule? Payload = default)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Result = Payload.Result, 
            RuleNumber = Payload.Number, 
            RuleName = Payload.Name, 
            Decision = Payload.Decision, 
            DecisionData = null, 
            StatementApplied = string.Empty,
            Message = ex.Message
        };
    }

    public static RightRuleResult<IProcessedRule> DefaultResult(IProcessedRule? Payload = default, string Message = "Default")
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Result = Payload.Result, 
            RuleNumber = Payload.Number, 
            RuleName = Payload.Name, 
            Decision = Payload.Decision, 
            DecisionData = Payload.DecisionData, 
            StatementApplied = string.Empty,
            Message = Message
        };
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