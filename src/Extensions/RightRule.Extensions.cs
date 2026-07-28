using RightRule.SyntaxChecker;

namespace RightRule.Extensions;

public static class RightRuleExtensions
{
    /// <summary>
    /// Standalone syntax checker for a RightRule statement returning rich diagnostics.
    /// Call as: var result = rightRuleInstance.checkstatement(statement);
    /// </summary>
    public async static Task<CheckStatementResult> CheckSyntaxAsync(this BeSoft.RightRule _, string statement)
    {
        return await StatementSyntaxChecker.CheckAsync(statement).ConfigureAwait(false);
    }
}