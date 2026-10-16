using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    private RightRuleServiceResult<IProcessedRule> BuildDefaultResult(string Context)
    {
        var rule =  RightRuleServiceResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context));

        var decisionData = InterpolatorService.Interpolate(rule.DecisionData);
        rule.DecisionData = decisionData;
        return rule;
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
            false => RightRuleServiceResult<IProcessedRule>.FailureResult(processedRule, DecisionData)
        };
        return result;
    }

    private object? MakeDecisionData(IProcessedRule rule)
    {
        var decisionData = rule.Result switch
        {
            true => InterpolatorService.Interpolate(rule.DecisionData),
            false => InterpolatorService.Interpolate(rule.FalseDecisionData)
        };
        return decisionData;
    }
}
