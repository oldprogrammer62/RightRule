using RightRule.Models;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.System.Common;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    /// <summary>
    /// Apply all Registered Rules for the given context to the registered data set(s).<br/>
    /// The results of all applied rules will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// All Rules applied are returned in a list.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    public List<RightRuleResult<IRule>> EvaluateAll(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return EvaluateAllAsync(Context).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the registered data set(s).<br/>
    /// The results of all applied rules will be returned.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// All Rules applied are returned in a list.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    public async Task<List<RightRuleResult<IRule>>> EvaluateAllAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateAllAsync(Context).ConfigureAwait(false);
            if (appliedRules == null || appliedRules.Count == 0)
            {
                return [RightRuleResult<IRule>.Default(DefaultRule.DefaultResult())];
            }
            return BuildResult(appliedRules);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the registered data set(s).<br/>
    /// The results returned are defined by the setting of RulesToReturn<br/>
    /// <b>First</b> will return a single Rule result<br/>
    /// <b>All</b> will return results of all Rules.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="ReturnRules">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified ReturnRules parameter.
    /// </returns>
    public List<RightRuleResult<IRule>> EvaluateAll(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return EvaluateAllAsync(Context, ReturnRules).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the registered data set(s).<br/>
    /// The results returned are defined by the setting of RulesToReturn<br/>
    /// <b>First</b> will return a single Rule result<br/>
    /// <b>All</b> will return results of all Rules.
    /// </summary>
    /// <param name="Context">Used to identify the set of rules to apply</param>
    /// <param name="ReturnRules">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified ReturnRules parameter.
    /// </returns>
    public async Task<List<RightRuleResult<IRule>>> EvaluateAllAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateAllAsync(Context, ReturnRules).ConfigureAwait(false);
            if (appliedRules == null || appliedRules.Count == 0)
            {
                return [RightRuleResult<IRule>.Default(DefaultRule.DefaultResult())];
            }
            return BuildResult(appliedRules);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given Context to the specified data set.<br/>
    /// The results of all applied rules will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public List<RightRuleResult<IRule>> EvaluateAll(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            return EvaluateAllAsync(Context, DataName, Data).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the specified data set.<br/>
    /// The results of all applied rules will be returned.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DataName">The name of the dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<List<RightRuleResult<IRule>>> EvaluateAllAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateAllAsync(Context, DataName, Data).ConfigureAwait(false);
            if (appliedRules == null || appliedRules.Count == 0)
            {
                return [RightRuleResult<IRule>.Default(DefaultRule.DefaultResult())];
            }
            return BuildResult(appliedRules);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the specified data set.<br/>
    /// The results returned are defined by the setting of RulesToReturn<br/>
    /// <b>First</b> will return a single Rule result<br/>
    /// <b>All</b> will return results of all Rules.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DataName">The name of the registered dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <param name="ReturnRules">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified ReturnRules parameter.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public List<RightRuleResult<IRule>> EvaluateAll(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            return EvaluateAllAsync(Context, DataName, Data, ReturnRules).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }

    /// <summary>
    /// Apply all Registered Rules for the given context to the specified data set.<br/>
    /// The results returned are defined by the setting of RulesToReturn<br/>
    /// <b>First</b> will return a single Rule result<br/>
    /// <b>All</b> will return results of all Rules.<br/>
    /// The data set provided will be registered and deregistered as part of this method call.
    /// </summary>
    /// <param name="Context">Valid types are: Dictionary, Enumerable, object with name/value pairs</param>
    /// <param name="DataName">The name of the registered dataset to use</param>
    /// <param name="Data">The data to apply the rule to</param>
    /// <param name="ReturnRules">Specifies if all rules applied are returned or the rule that returns true</param>
    /// <returns>
    /// A List of AppliedRule objects is returned.<br />
    /// The result of each rule can be checked in the Result property of each AppliedRule object.<br />
    /// Return is based on specified ReturnRules parameter.
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<List<RightRuleResult<IRule>>> EvaluateAllAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateAllAsync(Context, DataName, Data, ReturnRules).ConfigureAwait(false);
            if (appliedRules == null || appliedRules.Count == 0)
            {
                return [RightRuleResult<IRule>.Default(DefaultRule.DefaultResult())];
            }
            return BuildResult(appliedRules);
        }
        catch (Exception ex)
        {
            return [RightRuleResult<IRule>.Error(ex, ErrorRule.ErrorResult(ex.Message))];
        }
    }
}
