using RightRule.RuleServices.RuleModels.BaseModels;
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

    public static RightRuleResult<IProcessedRule> SuccessResult(IProcessedRule? Payload, string StatementApplied, object? DecisionData)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        {
            Result = Payload.Result, 
            RuleNumber = Payload.Number, 
            RuleName = Payload.Name, 
            Decision = Payload.TrueDecision, 
            DecisionData = DecisionData, 
            StatementApplied = StatementApplied 
        };
    }

    public static RightRuleResult<IProcessedRule> FailureResult(IProcessedRule? Payload, string StatementApplied, object? DecisionData, string Message)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Result = Payload.Result, 
            RuleNumber = Payload.Number, 
            RuleName = Payload.Name, 
            Decision = Payload.FalseDecision, 
            DecisionData = DecisionData, 
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
            Decision = Payload.TrueDecision, 
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
            Decision = Payload.TrueDecision, 
            DecisionData = Payload.TrueDecisionData, 
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