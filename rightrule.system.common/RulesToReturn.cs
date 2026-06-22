namespace RightRule.System.Common;

/// <summary>
/// Specifies which set of rules will return a successful result.
/// </summary>
/// <remarks>
/// First: The first rule that returns 'true' will be the result of the evaluation.
/// <br></br>
/// All: All rules will be returned. The result attribute of each rule will indicate rule success.
/// </remarks>
public enum RulesToReturn
{
    First = 0,
    All = 1
}
