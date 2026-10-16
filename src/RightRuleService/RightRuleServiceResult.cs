using RightRule.Models;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RightRuleService;

public sealed class RightRuleServiceResult<T>
{
    public required string Context { get; init; } 
    public required bool Success { get; init; }
    public required bool Result { get; init; }

    public required string Decision { get; init; }
    public object? DecisionData {  get; set; } // Needs to be settable. DecisionData can change by string interpolation process.

    public required string StatementApplied { get; init; }

    public RightRuleStatusEnum Status { get; init; }
    public T? Payload { get; init; }
    public string Message { get; init; } = string.Empty;
    public Exception? Exception { get; init; }

    private RightRuleServiceResult() { }

    public static RightRuleServiceResult<IProcessedRule> SuccessResult(IProcessedRule Payload, object? DecisionData)
    {
        return new RightRuleServiceResult<IProcessedRule> 
        { 
            Status = RightRuleStatusEnum.SUCCESS, 
            Success = Payload.Success, 
            Result = Payload.Result, 
            Context = Payload.Context,
            StatementApplied = Payload.StatementApplied, 
            Decision = Payload.Decision, 
            DecisionData = DecisionData, 
            Payload = Payload,
            Message = string.Empty
        };
    }

    public static RightRuleServiceResult<IProcessedRule> FailureResult(IProcessedRule? Payload, object? DecisionData)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Status = RightRuleStatusEnum.FAILURE, 
            Success = Payload.Success, 
            Result = Payload.Result, 
            Context = Payload.Context, 
            StatementApplied = Payload.StatementApplied, 
            Decision = Payload.FalseDecision, 
            DecisionData = DecisionData, 
            Payload = Payload,
            Message = Payload.Message
        };
    }

    internal static RightRuleServiceResult<IProcessedRule> ErrorResult(Exception ex, IProcessedRule? Payload)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Status = RightRuleStatusEnum.ERROR, 
            Success = Payload.Success, 
            Result = Payload.Result, 
            Context = Payload.Context,
            StatementApplied = Payload.StatementApplied, 
            Decision = Payload.Decision, 
            DecisionData = null, 
            Payload = Payload,
            Message = ex.Message,
            Exception = ex 
        };
    }

    internal static RightRuleServiceResult<IProcessedRule> DefaultResult(IProcessedRule? Payload, string Message = "Default")
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Status = RightRuleStatusEnum.DEFAULT, 
            Success = Payload.Success, 
            Result = Payload.Result, 
            Context = Payload.Context,
            StatementApplied = Payload.StatementApplied, 
            Decision = Payload.Decision, 
            DecisionData = Payload.DecisionData,
            Payload = Payload, 
            Message = Message
        };
    }
}
