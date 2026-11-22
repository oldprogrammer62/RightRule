using RightRule.Models;
using RightRule.RightRuleService;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    private RightRuleServiceResult<IProcessedRule> BuildDefaultResult(string Context)
    {
        var rule = RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context));

        var decisionData = _interpolatorService.Interpolate(rule.DecisionData);
        rule.DecisionData = decisionData;
        return rule;
    }

    private List<RightRuleResult<IProcessedRule>> BuildResults(string Context, List<RightRuleServiceResult<IProcessedRule>> ServiceResults)
    {
        if (ServiceResults == null)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
        }
        if (ServiceResults.Count == 0)
        {
            return [RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
        }
        List<RightRuleResult<IProcessedRule>> rightRuleResults = new List<RightRuleResult<IProcessedRule>>();

        foreach (var serviceResult in ServiceResults)
        {
            rightRuleResults.Add(BuildResult(Context, serviceResult));
        }
        return rightRuleResults;
    }

    private RightRuleResult<IProcessedRule> BuildResult(string Context, RightRuleServiceResult<IProcessedRule> ServiceResult)
    {
        var status = ServiceResult?.Status ?? RightRuleStatusEnum.DEFAULT;

        var payload = ServiceResult?.Payload ?? null;
        var message = ServiceResult?.Message ?? string.Empty;
        var statementApplied = ServiceResult?.Payload?.StatementApplied ?? string.Empty;
        var decisionData = ServiceResult?.DecisionData ?? string.Empty;

        return status switch
        {
            RightRuleStatusEnum.SUCCESS when ServiceResult?.Payload != null =>
                RightRuleResult<IProcessedRule>.SuccessResult(payload, statementApplied, decisionData),

            RightRuleStatusEnum.DEFAULT =>
                RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context), message),

            RightRuleStatusEnum.ERROR =>
                RightRuleResult<IProcessedRule>.ErrorResult(ServiceResult?.Exception ?? new Exception(message), payload != null ? payload : ErrorRule.ErrorResult(message)),

            _ => RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))
        };
    }

    private List<RightRuleServiceResult<IProcessedRule>> BuildResult(List<IProcessedRule> RulesApplied)
    {
        List<RightRuleServiceResult<IProcessedRule>> results = new List<RightRuleServiceResult<IProcessedRule>>();
        foreach (var ruleApplied in RulesApplied)
        {
            var decisionData = MakeDecisionData(ruleApplied);
            results.Add(BuildResultObject(ruleApplied, decisionData));
        }
        return results;
    }

    private static RightRuleServiceResult<IProcessedRule> BuildResultObject(IProcessedRule processedRule, object? DecisionData)
    {
        var result = processedRule.Result switch
        {
            true => RightRuleServiceResult<IProcessedRule>.SuccessResult(processedRule, DecisionData),
            false => RightRuleServiceResult<IProcessedRule>.ErrorResult(processedRule, processedRule.Message)
        };
        return result;
    }

    private object? MakeDecisionData(IProcessedRule rule)
    {
        return _interpolatorService.Interpolate(rule.TrueDecisionData);
    }
}