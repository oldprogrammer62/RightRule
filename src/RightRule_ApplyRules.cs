using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;

namespace BeSoft;

public sealed partial class RightRule : IRightRule, IDisposable
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
            var svcResult = await ApplyRulesAsync_SVC(Context).ConfigureAwait(false);
            if (svcResult == null)
            {
                return RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context));
            }
            return svcResult;
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
            var svcResult = await ApplyRulesAsync_SVC(Context, EvaluationTypeEnum.FIRST).ConfigureAwait(false);
            if (svcResult == null)
            {
                return [RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context))];
            }
            return svcResult;
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
    /// <param name="evaluationType"></param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, EvaluationTypeEnum evaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRulesAsync(Context, evaluationType).GetAwaiter().GetResult();
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
    /// <param name="evaluationType"></param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var svcResult = await ApplyRulesAsync_SVC(Context, evaluationType).ConfigureAwait(false);
            if (svcResult == null)
            {
                return [RightRuleResult<IProcessedRule>.DefaultResult(RightRuleService.GetDefaultRule(Context))];
            }
            return svcResult;
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    // ***************************************************************
    // ***************** CALLS TO RIGHT RULE SERVICE *****************
    // ***************************************************************

    private async Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync_SVC(string Context)
    {
        try
        {
            var rulesApplied = await RightRuleService.ApplyRulesAsync(Context);
            return BuildResult(rulesApplied);
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    private async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync_SVC(string Context, EvaluationTypeEnum evaluationType)
    {
        try
        {
            var rulesApplied = await RightRuleService.ApplyRulesAsync(Context, evaluationType);
            return BuildResults(Context, rulesApplied);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    //private async Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync_SVC(string Context, string DataName, Dictionary<string, object> Data, DataRetentionEnum dataRetention)
    //{
    //    try
    //    {
    //        var rulesApplied = await RightRuleService.ApplyRulesAsync(Context, DataName, Data, EvaluationTypeEnum.FIRST, dataRetention);
    //        return BuildResult(Context, rulesApplied);
    //    }
    //    catch (Exception ex)
    //    {
    //        return RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message));
    //    }
    //}

    //private async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync_SVC(string Context, string DataName, Dictionary<string, object> Data, EvaluationTypeEnum rulesToReturn, DataRetentionEnum dataRetention)
    //{
    //    try
    //    {
    //        var rulesApplied = await RightRuleService.ApplyRulesAsync(Context, DataName, Data, rulesToReturn, dataRetention);
    //        return BuildResults(Context, rulesApplied);
    //    }
    //    catch (Exception ex)
    //    {
    //        return [RightRuleResult<IProcessedRule>.ErrorResult(ex, ErrorRule.ErrorResult(ex.Message))];
    //    }
    //}
}
