using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RightRule.DataServices;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RightRuleService;
using RightRule.RuleServices;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using System.ComponentModel;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    private List<RightRuleResult<IProcessedRule>> BuildResults(string Context, List<RightRuleServiceResult<IProcessedRule>> ServiceResults)
    {
        if (ServiceResults == null)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context))];
        }
        if (ServiceResults.Count == 0)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context))];
        }
        List<RightRuleResult<IProcessedRule>> rightRuleResults = new List<RightRuleResult<IProcessedRule>>();

        foreach (var serviceResult in ServiceResults)
        {
            rightRuleResults.Add(BuildResult(serviceResult));
        }
        return rightRuleResults;
    }

    private RightRuleResult<IProcessedRule> BuildResult(RightRuleServiceResult<IProcessedRule> ServiceResult)
    {
        var context = ServiceResult?.Payload?.Context ?? string.Empty;
        var status = ServiceResult?.Status ?? RightRuleStatusEnum.DEFAULT;

        var payload = ServiceResult?.Payload ?? null;
        var message = ServiceResult?.Message ?? string.Empty;
        var statementApplied = ServiceResult?.Payload?.StatementApplied ?? string.Empty;

        return status switch
        {
            RightRuleStatusEnum.SUCCESS when ServiceResult?.Payload != null =>
                RightRuleResult<IProcessedRule>.SuccessResult(payload, statementApplied),

            RightRuleStatusEnum.FAILURE =>
                RightRuleResult<IProcessedRule>.FailureResult(message, statementApplied, payload != null ? payload : RightRuleService.GetDefaultRule(context)),

            RightRuleStatusEnum.DEFAULT =>
                RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(context), message),

            RightRuleStatusEnum.ERROR =>
                RightRuleResult<IProcessedRule>.ErrorResult(ServiceResult?.Exception ?? new Exception(message), payload != null ? payload : ErrorRule.ErrorResult(message)),

            _ => RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(context))
        };
    }
}
