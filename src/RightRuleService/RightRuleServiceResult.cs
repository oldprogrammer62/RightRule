using RightRule.Models.BaseModels;
using RightRule.RegistrationServices.RuleRegistration;

namespace RightRule.RightRuleService;

internal sealed class RightRuleServiceResult<T>
{
    public required string Context { get; init; } 
    public required bool Success { get; init; }
    public required bool Result { get; init; }

    public required string Decision { get; init; }
    public object? DecisionData {  get; init; }

    public required string StatementApplied { get; init; }

    public RegistrationStatus Status { get; init; }
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
        return new RightRuleServiceResult<IProcessedRule> 
        { 
            Status = RegistrationStatus.SUCCESS, 
            Success = payload.Success, 
            Result = payload.Result, 
            Context = payload.Context,
            StatementApplied = payload.StatementApplied, 
            Decision = payload.Decision, 
            DecisionData = payload.DecisionData, 
            Payload = payload,
            Message = string.Empty
        };
    }

    public static RightRuleServiceResult<IProcessedRule> Build(IProcessedRule payload)
    {
        return payload.Result switch
        {
            true => SuccessResult(payload),
            false => FailureResult(payload)
        };
    }

    public static RightRuleServiceResult<IProcessedRule> FailureResult(IProcessedRule? payload)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() 
        { 
            Status = RegistrationStatus.FAILURE, 
            Success = payload.Success, 
            Result = payload.Result, 
            Context = payload.Context, 
            StatementApplied = payload.StatementApplied, 
            Decision = payload.FalseDecision, 
            DecisionData = payload.FalseDecisionData, 
            Payload = payload,
            Message = payload.Message
        };
    }

    public static RightRuleServiceResult<IProcessedRule> ErrorResult(Exception ex, IProcessedRule? payload)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() 
        { 
            Status = RegistrationStatus.ERROR, 
            Success = payload.Success, 
            Result = payload.Result, 
            Context = payload.Context,
            StatementApplied = payload.StatementApplied, 
            Decision = payload.Decision, 
            DecisionData = null, 
            Payload = payload,
            Message = ex.Message,
            Exception = ex 
        };
    }

    public static RightRuleServiceResult<IProcessedRule> DefaultResult(IProcessedRule? payload, string message = "Default")
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));
        return new() 
        { 
            Status = RegistrationStatus.DEFAULT, 
            Success = payload.Success, 
            Result = payload.Result, 
            Context = payload.Context,
            StatementApplied = payload.StatementApplied, 
            Decision = payload.Decision, 
            DecisionData = payload.DecisionData,
            Payload = payload, 
            Message = message
        };
    }
}
