using RightRule.LanguageProcessor.SyntaxChecker;

namespace RightRule.RightRuleSession;

public sealed partial class RightRuleSession : IRightRuleSession
{
    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    public CheckStatementResult StatementSyntaxCheck(string Statement)
    {
        if (string.IsNullOrWhiteSpace(Statement))
        {
            return CheckStatementResult.FailureResult();
        }
        return StatementSyntaxCheckAsync(Statement).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<CheckStatementResult> StatementSyntaxCheckAsync(string Statement)
    {
        if (string.IsNullOrWhiteSpace(Statement))
        {
            return CheckStatementResult.FailureResult();
        }
        return await _rightRuleService.SyntaxCheckAsync(Statement).ConfigureAwait(false);
    }
}
