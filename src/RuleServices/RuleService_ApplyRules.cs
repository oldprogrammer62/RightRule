using RightRule.LanguageProcessor.Evaluator;
using RightRule.RightRuleService;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.RuleServices;

internal sealed partial class RuleService : IRuleService
{
    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public List<RightRuleResult<IProcessedRule>> ApplyRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRulesAsync(Context, EvaluationTypeEnum.FIRST).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="EvaluationType"></param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRulesAsync(Context, EvaluationType).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var rulesApplied = await ApplyRules_MultipleResultAsync(Context, EvaluationTypeEnum.FIRST).ConfigureAwait(false);
            if (rulesApplied == null)
            {
                return [RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context))];
            }
            return BuildResults(Context, rulesApplied);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="EvaluationType"></param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var rulesApplied = await ApplyRules_MultipleResultAsync(Context, EvaluationType).ConfigureAwait(false);
            return BuildResults(Context, rulesApplied);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public RightRuleResult<IProcessedRule> ApplyRule(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRuleAsync(Context).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<RightRuleResult<IProcessedRule>> ApplyRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var rulesApplied = await ApplyRules_SingleResultAsync(Context, EvaluationTypeEnum.FIRST).ConfigureAwait(false);
            if (rulesApplied == null)
            {
                return RightRuleResult<IProcessedRule>.DefaultResult(GetDefaultRule(Context));
            }
            return BuildResult(Context, rulesApplied);
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    private async Task<RightRuleServiceResult<IProcessedRule>> ApplyRules_SingleResultAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await ApplyRules_MultipleResultAsync(Context, EvaluationType).ConfigureAwait(false);
        if (rulesApplied == null || rulesApplied.Count == 0)
        {
            return BuildDefaultResult(Context);
        }
        var result = rulesApplied.FirstOrDefault();
        if (result != null)
        {
            return result;
        }
        return BuildDefaultResult(Context);
    }

    private async Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRules_MultipleResultAsync(string Context, EvaluationTypeEnum EvaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        var rulesApplied = await _ruleApplicationService.ApplyRulesAsync(Context, EvaluationType).ConfigureAwait(false);
        if (rulesApplied == null)
        {
            return [BuildDefaultResult(Context)];
        }
        return BuildResult(rulesApplied);
    }
}