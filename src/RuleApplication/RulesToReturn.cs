namespace RightRule.RuleApplication;

/// <summary>
/// Specifies which set of rules will return a successful result.
/// </summary>
/// <remarks>
/// First: The first rule that returns 'true' will be the result of the evaluation.
/// Only one rule will be returned.
/// The returned rule will be either a true Decision or the Default Rule Decision
/// <br></br>
/// All: All rules will be returned. The result attribute of each rule will indicate rule success.
/// All rules applied will be returned.
/// The Result attribute will indicate a true or false result of the statement.
/// Each Rule will return either its true Decision details or False Decision details.
/// </remarks>
public enum RulesToReturn
{
    First = 0,
    All = 1
}
