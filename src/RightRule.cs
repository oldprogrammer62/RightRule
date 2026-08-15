using RightRule.Data;
using RightRule.Extensions;
using RightRule.Models;
using RightRule.RuleBuilder;
using RightRule.RuleModels;
using RightRule.SyntaxChecker;
using RightRule.System.Common;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed class RightRule : IRightRule
{
    private readonly RightRuleService _rightRuleService;

    public RightRuleConfiguration RightRuleConfiguration => _rightRuleService.RightRuleConfig;

    public RightRule()
    {
        _rightRuleService = new RightRuleService();
    }

    /// <summary>
    /// Build an enhanced Rule Definition
    /// DataCheck - If set the Parser will attempt to validate data references with data reqgistrations
    /// Diagnostics - If set additional data will be returned with the Parsed Rule and Applied Rule objects
    /// </summary>
    /// <param name="RuleDefn"></param>
    /// <returns>RuleBuilder</returns>
    public RuleBuilder BuildRule(RuleDefinition RuleDefn)
    {
        return RuleBuilder.Create(RuleDefn);
    }

    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<CheckStatementResult> SyntaxCheckAsync(string Statement)
    {
        if (string.IsNullOrWhiteSpace(Statement))
        {
            return CheckStatementResult.Failure();
        }
        return await this.CheckSyntaxAsync(Statement).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns the set of Operators currently loaded in the RightRule engine.
    /// </summary>
    /// <returns>The list of Operators.</returns>
    public List<Operator>? GetOperatorDetails()
    {
        var operators = _rightRuleService?.GetOperatorDetails();
        return operators;
    }

    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Name.
    /// </summary>
    /// <param name="Name"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? RegisteredRule(string Name)
    {
        try
        {
            return _rightRuleService.GetRegisteredRule(Name);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the Registered (Parsed) Rules with the specified Number.
    /// </summary>
    /// <param name="Number"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public ParsedRule? RegisteredRule(int Number)
    {
        try
        {
            return _rightRuleService.GetRegisteredRule(Number);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules.
    /// </summary>
    public RegisteredRules RegisteredRules()
    {
        try
        {
            return _rightRuleService.RegisteredRules;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered (Parsed) Rules for the specified Context.
    /// </summary>
    /// <param name="Context"></param>
    /// <exception cref="ArgumentException"></exception>
    public List<ParsedRule> RegisteredRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        try
        {
            return _rightRuleService.GetRules(Context);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a set of Rules with the RightRule engine.
    /// </summary>
    /// <param name="rules">The Rule definitions to register.</param>
    /// <returns>The set of Parsed Rules.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        try
        {
            return _rightRuleService.RegisterRules(rules);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a single Rule with the RightRule engine.
    /// </summary>
    /// <param name="rule">The Rule definition to register.</param>
    /// <returns>The Parsed Rule.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public ParsedRule RegisterRule(RuleDefinition rule)
    {
        ArgumentNullException.ThrowIfNull(rule, nameof(rule));
        try
        {
            return _rightRuleService.RegisterRule(rule);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregisters all Rules associated with the specified Context from the RightRule engine.
    /// </summary>
    /// <param name="Context">The Context for which to deregister rules.</param>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        try
        {
            return _rightRuleService.DeRegisterRules(Context);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregisters all Rules from the RightRule engine.
    /// </summary>
    /// <returns>True if the rules were successfully deregistered; otherwise, false.</returns>
    public bool DeRegisterRules()
    {
        try
        {
            return _rightRuleService.DeRegisterRules();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Name">The Name of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRule(string Name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name, nameof(Name));
        try
        {
            return _rightRuleService.DeRegisterRule(Name);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Deregister the specified Rule from the RightRule engine.
    /// </summary>
    /// <param name="Number">The Number of the Rule to deregister.</param>
    /// <returns>True if the rule is successfully deregistered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterRule(int Number)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Number.ToString(), nameof(Number));
        try
        {
            return _rightRuleService.DeRegisterRule(Number);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the specified Dataset.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <returns>RegisteredDataset object</returns>
    /// <exception cref="ArgumentException"></exception>
    public RegisteredDataset? RegisteredDataset(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        try
        {
            return _rightRuleService.GetDataSet(DataName);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the specified DataObject
    /// </summary>
    /// <param name="DataName">The name of the dataobject.</param>
    /// <returns>RegisteredDataObject</returns>
    /// <exception cref="ArgumentException"></exception>
    public RegisteredDataObject? RegisteredDataObject(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        try
        {
            return _rightRuleService.GetDataObject(DataName);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered Datasets.
    /// </summary>
    /// <returns>RegisteredDatasets object</returns>
    public RegisteredDatasets RegisteredDatasets()
    {
        try
        {
            return _rightRuleService.RegisteredDatasets;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Returns the set of Registered DataObjects
    /// </summary>
    /// <returns>RegisteredDataObjects object</returns>
    public RegisteredDataObjects RegisteredDataObjects()
    {
        try
        {
            return _rightRuleService.RegisteredDataObjects;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="Data">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public bool RegisterData(string DataName, Dictionary<string, object> Data)
    {
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        try
        {
            return _rightRuleService.RegisterData(DataName, Data);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Registers a data set with the RightRule engine for use in rule evaluation.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <param name="DataReference">The data to register.</param>
    /// <returns>True if the data was successfully registered; otherwise, false.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public bool RegisterData(string DataName, object DataReference)
    {
        ArgumentNullException.ThrowIfNull(DataReference, nameof(DataReference));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        try
        {
            return _rightRuleService.RegisterData(DataName, DataReference);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// DeRegisters all data from the RightRule engine.
    /// </summary>
    public bool DeRegisterData()
    {
        try
        {
            return _rightRuleService.DeRegisterData();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// DeRegisters the specified data from the RightRule engine.
    /// </summary>
    /// <param name="DataName">The name of the dataset.</param>
    /// <exception cref="ArgumentException"></exception>
    public bool DeRegisterData(string DataName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));

        try
        {
            return _rightRuleService.DeRegisterData(DataName);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Unloads an Operator from the RightRule engine.
    /// The Operator will no longer be valid in a Rule Statement
    /// </summary>
    /// <param name="OperatorName">The name of the Operator to unload.</param>
    /// <exception cref="ArgumentException"></exception>
    public bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));

        try
        {
            return _rightRuleService.UnloadOperator(OperatorName);
        }
        catch
        {
            throw;
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
    public AppliedRule EvaluateFirstRule(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return EvaluateFirstRuleAsync(Context).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return AppliedRule.ErrorResult(ex.Message);
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
    public async Task<AppliedRule> EvaluateFirstRuleAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateFirstRuleAsync(Context).ConfigureAwait(false);
            if (appliedRules == null)
            {
                return AppliedRule.DefaultResult();
            }
            if (appliedRules.Count == 1)
            {
                return appliedRules[0];
            }
            return AppliedRule.DefaultResult();
        }
        catch (Exception ex)
        {
            return AppliedRule.ErrorResult(ex.Message);
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
    public AppliedRule EvaluateFirstRule(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            return EvaluateFirstRuleAsync(Context, DataName, Data).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return AppliedRule.ErrorResult(ex.Message);
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
    public async Task<AppliedRule> EvaluateFirstRuleAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateFirstRuleAsync(Context, DataName, Data).ConfigureAwait(false);
            if (appliedRules == null)
            {
                return AppliedRule.DefaultResult();
            }
            if (appliedRules.Count == 1)
            {
                return appliedRules[0];
            }
            return AppliedRule.DefaultResult();
        }
        catch (Exception ex)
        {
            return AppliedRule.ErrorResult(ex.Message);
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
    public List<AppliedRule> EvaluateAllRules(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return EvaluateAllRulesAsync(Context).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [AppliedRule.ErrorResult(ex.Message)];
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
    public async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateAllRulesAsync(Context).ConfigureAwait(false);
            if (appliedRules == null)
            {
                return [AppliedRule.DefaultResult()];
            }
            if (appliedRules.Count > 0)
            {
                return appliedRules;
            }
            return [AppliedRule.DefaultResult()];
        }
        catch (Exception ex)
        {
            return [AppliedRule.ErrorResult(ex.Message)];
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
    public List<AppliedRule> EvaluateAllRules(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            return EvaluateAllRulesAsync(Context, ReturnRules).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [AppliedRule.ErrorResult(ex.Message)];
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
    public async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateAllRulesAsync(Context, ReturnRules).ConfigureAwait(false);
            if (appliedRules == null)
            {
                return [AppliedRule.DefaultResult()];
            }
            if (appliedRules.Count > 0)
            {
                return appliedRules;
            }
            return [AppliedRule.DefaultResult()];
        }
        catch (Exception ex)
        {
            return [AppliedRule.ErrorResult(ex.Message)];
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
    public List<AppliedRule> EvaluateAllRules(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            return EvaluateAllRulesAsync(Context, DataName, Data).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            return [AppliedRule.ErrorResult(ex.Message)];
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
    public async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateAllRulesAsync(Context, DataName, Data).ConfigureAwait(false);
            if (appliedRules == null)
            {
                return [AppliedRule.DefaultResult()];
            }
            if (appliedRules.Count > 0)
            {
                return appliedRules;
            }
            return [AppliedRule.DefaultResult()];
        }
        catch (Exception ex)
        {
            return [AppliedRule.ErrorResult(ex.Message)];
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
    public List<AppliedRule> EvaluateAllRules(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            return EvaluateAllRulesAsync(Context, DataName, Data, ReturnRules).GetAwaiter().GetResult();
        }
                catch (Exception ex)
        {
            return [AppliedRule.ErrorResult(ex.Message)];
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
    public async Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Context, nameof(Context));
        ArgumentException.ThrowIfNullOrWhiteSpace(DataName, nameof(DataName));
        ArgumentNullException.ThrowIfNull(Data, nameof(Data));

        try
        {
            var appliedRules = await _rightRuleService.EvaluateAllRulesAsync(Context, DataName, Data, ReturnRules).ConfigureAwait(false);
            if (appliedRules == null)
            {
                return [AppliedRule.DefaultResult()];
            }
            if (appliedRules.Count > 0)
            {
                return appliedRules;
            }
            return [AppliedRule.DefaultResult()];
        }
        catch (Exception ex)
        {
            return [AppliedRule.ErrorResult(ex.Message)];
        }
    }
}
