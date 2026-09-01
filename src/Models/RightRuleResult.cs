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
    public required int RuleNumber { get; init; }
    public required string RuleName { get; init; }

    //public required bool IsSuccess { get; init; }
    //public required bool Result { get; init; }

    public required string Decision { get; init; }
    public object? DecisionData { get; init; }
    
    public string StatementApplied { get; init; }
    
    public string Message { get; init; } = string.Empty;

    //public Exception? Exception { get; init; }

    private RightRuleResult() { }

    public static RightRuleResult<IProcessedRule> Success(IProcessedRule? payload, string statementApplied) =>
        new() { RuleNumber = payload.Number, RuleName = payload.Name, Decision = payload.Decision, DecisionData = payload.DecisionData, StatementApplied = statementApplied };

    public static RightRuleResult<IProcessedRule> Failure(string message, string statementApplied, IProcessedRule? payload = default) =>
        new() { RuleNumber = payload.Number, RuleName = payload.Name, Message = message, Decision = payload.FalseDecision, DecisionData = payload?.FalseDecisionData, StatementApplied = statementApplied };

    public static RightRuleResult<IProcessedRule> Error(Exception ex, IProcessedRule? payload = default) =>
        new() { RuleNumber = payload.Number, RuleName = payload.Name, Message = ex.Message, Decision = payload.Decision, DecisionData = null, StatementApplied = string.Empty};

    public static RightRuleResult<IProcessedRule> Default(IProcessedRule? payload = default, string message = "Default") =>
        new() { RuleNumber = payload.Number, RuleName = payload.Name, Message = message, Decision = payload.Decision, DecisionData = payload.DecisionData, StatementApplied = string.Empty };
}