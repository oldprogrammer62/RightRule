using RightRule.OperatorServices;
using RightRule.System.Common.Operator.Models;

namespace RightRule;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
    public OperatorRegistrationResult RegisterOperator(Operator Operator)
    {
        return RightRuleService.RegisterOperator(Operator);
    }

    /// <summary>
    /// Returns the set of Operators currently loaded in the RightRule engine.
    /// </summary>
    /// <returns>The list of Operators.</returns>
    public List<Operator>? GetOperators()
    {
        var operators = RightRuleService?.GetOperators();
        return operators;
    }

    /// <summary>
    /// Returns the set of Aliases for logic Operators
    /// </summary>
    /// <returns>The list of Operator Aliases.</returns>
    public Dictionary<string, Operator>? GetOperatorAliases()
    {
        var operators = RightRuleService?.GetOperatorAliases();
        return operators;
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
            return RightRuleService.UnloadOperator(OperatorName);
        }
        catch
        {
            throw;
        }
    }
}
