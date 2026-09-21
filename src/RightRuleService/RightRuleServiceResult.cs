using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration;

namespace RightRule.RightRuleService;

public sealed class RightRuleServiceResult<T>
{
    public required string Context { get; init; } 
    public required bool Success { get; init; }
    public required bool Result { get; init; }

    public required string Decision { get; init; }
    public object? DecisionData {  get; init; }

    public required string StatementApplied { get; init; }

    public RegistrationStatusEnum Status { get; init; }
    public T? Payload { get; init; }
    public string Message { get; init; } = string.Empty;
    public Exception? Exception { get; init; }

    private RightRuleServiceResult() { }

    public static List<RightRuleServiceResult<IProcessedRule>> SuccessResult(List<IProcessedRule> Payloads)
    {
        List<RightRuleServiceResult<IProcessedRule>> results = [];

        foreach (var payload in Payloads)
        {
            var result = SuccessResult(payload);
            results.Add(result);
        }
        return results;
    }

    public static RightRuleServiceResult<IProcessedRule> SuccessResult(IProcessedRule Payload)
    {
        return new RightRuleServiceResult<IProcessedRule> 
        { 
            Status = RegistrationStatusEnum.SUCCESS, 
            Success = Payload.Success, 
            Result = Payload.Result, 
            Context = Payload.Context,
            StatementApplied = Payload.StatementApplied, 
            Decision = Payload.Decision, 
            DecisionData = Payload.DecisionData, 
            Payload = Payload,
            Message = string.Empty
        };
    }

    public static RightRuleServiceResult<IProcessedRule> Build(IProcessedRule Payload)
    {
        return Payload.Result switch
        {
            true => SuccessResult(Payload),
            false => FailureResult(Payload)
        };
    }

    public static RightRuleServiceResult<IProcessedRule> FailureResult(IProcessedRule? Payload)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Status = RegistrationStatusEnum.FAILURE, 
            Success = Payload.Success, 
            Result = Payload.Result, 
            Context = Payload.Context, 
            StatementApplied = Payload.StatementApplied, 
            Decision = Payload.FalseDecision, 
            DecisionData = Payload.FalseDecisionData, 
            Payload = Payload,
            Message = Payload.Message
        };
    }

    public static RightRuleServiceResult<IProcessedRule> ErrorResult(Exception ex, IProcessedRule? Payload)
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Status = RegistrationStatusEnum.ERROR, 
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

    public static RightRuleServiceResult<IProcessedRule> DefaultResult(IProcessedRule? Payload, string Message = "Default")
    {
        ArgumentNullException.ThrowIfNull(Payload, nameof(Payload));
        return new() 
        { 
            Status = RegistrationStatusEnum.DEFAULT, 
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
