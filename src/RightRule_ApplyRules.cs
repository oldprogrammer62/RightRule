using RightRule.Models;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.System.Common;

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
    public RightRuleResult<IProcessedRule> ApplyRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRulesAsync(Context).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
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
    public async Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var svcResult = await ApplyRulesAsync_SVC(Context).ConfigureAwait(false);
            if (svcResult == null)
            {
                return RightRuleResult<IProcessedRule>.Default(DefaultRule.DefaultResult());
            }
            return svcResult;
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="rulesToReturn"></param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return ApplyRulesAsync(Context, rulesToReturn).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the registered data set(s).<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="rulesToReturn"></param>
    /// <returns>
    /// A single Applied Rule is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var svcResult = await ApplyRulesAsync_SVC(Context, rulesToReturn).ConfigureAwait(false);
            if (svcResult == null)
            {
                return [RightRuleResult<IProcessedRule>.Default(DefaultRule.DefaultResult())];
            }
            return svcResult;
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the data set provided.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    public RightRuleResult<IProcessedRule> ApplyRules(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            return ApplyRulesAsync(Context, DataName, Data).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the data set provided.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    public async Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            var svcResult = await ApplyRulesAsync_SVC(Context, DataName, Data).ConfigureAwait(false);
            if (svcResult == null)
            {
                return RightRuleResult<IProcessedRule>.Default(DefaultRule.DefaultResult());
            }
            return svcResult;
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    // ***************** FROM EVALUATE ALL RULES *****************

    /// <summary>
    /// Apply all Registered Rules for the given context to the data set provided.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    public List<RightRuleResult<IProcessedRule>> ApplyRules(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            return ApplyRulesAsync(Context, DataName, Data, rulesToReturn).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the data set provided.<br/>
    /// The first Rule to return a positive (true) result will be returned.<br/>
    /// If no rule returns a positive result, a default rule will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <param name="rulesToReturn"></param>
    /// <returns>
    /// A single rule object is returned.<br />
    /// If a rule returns true, that rule is returned.<br />
    /// If no rule returns true, a default rule is returned.<br />
    /// </returns>
    public async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            var svcResult = await ApplyRulesAsync_SVC(Context, DataName, Data, rulesToReturn).ConfigureAwait(false);
            if (svcResult == null)
            {
                return [RightRuleResult<IProcessedRule>.Default(DefaultRule.DefaultResult())];
            }
            return svcResult;
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }


    // ***************** CALLS TO RIGHT RULE SERVICE *****************

    private async Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync_SVC(string Context)
    {
        try
        {
            var rulesApplied = await _rightRuleService.ApplyRulesAsync(Context);
            return BuildResult(rulesApplied);
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    private async Task<RightRuleResult<IProcessedRule>> ApplyRulesAsync_SVC(string Context, string DataName, Dictionary<string, object> Data)
    {
        try
        {
            var rulesApplied = await _rightRuleService.ApplyRulesAsync(Context, DataName, Data);
            return BuildResult(rulesApplied);
        }
        catch (Exception ex)
        {
            return RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }

    private async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync_SVC(string Context, RulesToReturn rulesToReturn)
    {
        try
        {
            var rulesApplied = await _rightRuleService.ApplyRulesAsync(Context, rulesToReturn);
            return BuildResult(rulesApplied);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    private async Task<List<RightRuleResult<IProcessedRule>>> ApplyRulesAsync_SVC(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn)
    {
        try
        {
            var rulesApplied = await _rightRuleService.ApplyRulesAsync(Context, DataName, Data, rulesToReturn);
            return BuildResult(rulesApplied);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IProcessedRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }
}
