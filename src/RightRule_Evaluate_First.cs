using RightRule.Models;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
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
    public RightRuleResult<IRule> EvaluateFirstMatch(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return EvaluateFirstMatchAsync(Context).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
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
    public async Task<RightRuleResult<IRule>> EvaluateFirstMatchAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var svcResult = await _rightRuleService.EvaluateFirstAsync(Context).ConfigureAwait(false);
            if (svcResult == null)
            {
                return RightRuleResult<IRule>.Default(DefaultRule.DefaultResult());
            }
            return BuildResult(svcResult);
        }
        catch (Exception ex)
        {
            return RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
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
    public RightRuleResult<IRule> EvaluateFirstMatch(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            return EvaluateFirstMatchAsync(Context, DataName, Data).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
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
    public async Task<RightRuleResult<IRule>> EvaluateFirstMatchAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            var svcResult = await _rightRuleService.EvaluateFirstMatchWithDataAsync(Context, DataName, Data).ConfigureAwait(false);

            return BuildResult(svcResult);
        }
        catch (Exception ex)
        {
            return RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message));
        }
    }
}
