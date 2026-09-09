using RightRule.Extensions;
using RightRule.SyntaxChecker;

namespace BeSoft;

/// <summary>
/// Main RightRule class
/// Provides all public entry points to the RightRule system
/// </summary>
public sealed partial class RightRule : IRightRule, IDisposable
{
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
            return CheckStatementResult.FailureResult();
        }
        return await this.CheckSyntaxAsync(Statement).ConfigureAwait(false);
    }
}
