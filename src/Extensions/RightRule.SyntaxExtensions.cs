using RightRule.StatementProcessor;

namespace RightRule;

public static class RightRuleSyntaxExtensions
{
    /// <summary>
    /// Standalone syntax checker for a RightRule statement returning rich diagnostics.
    /// Call as: var result = rightRuleInstance.checkstatement(statement);
    /// </summary>
    public static CheckStatementResult checkstatement(this RightRule _, string statement)
    {
        var checker = new StatementSyntaxChecker();
        return checker.Check(statement);
    }
}