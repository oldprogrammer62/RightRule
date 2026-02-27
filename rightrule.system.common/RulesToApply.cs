namespace RightRule.System.Common;

/// <summary>
/// Specifies which set of rules will return a successful result.
/// </summary>
/// <remarks>
/// First: The first rule that returns 'true' will be the result of the evaluation.
/// <br></br>
/// All: All rules must return 'true' for the evaluation to be successful.
/// </remarks>
public enum RulesToApply
{
    First = 0,
    All = 1
}
