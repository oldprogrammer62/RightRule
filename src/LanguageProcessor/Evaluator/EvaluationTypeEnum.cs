namespace RightRule.LanguageProcessor.Evaluator;

/// <summary>
/// Specifies which set of rules will return a successful result.
/// </summary>
/// <remarks>
/// FIRST
/// All rules will be processed.
/// The first rule that returns 'true' will be the result of the evaluation.
/// If no rule returns a true result, the Default rule will be returned.
/// Only one rule will be returned.
/// <br></br>
/// ANY
/// All rules will be processed.
/// The result attribute of each rule will indicate rule success.
/// Each Rule will return either its True Decision details or False Decision details.
/// All rules applied will be returned.
/// <br></br>
/// ALL
/// All rules will be processed.
/// All rules applied will be returned only if all rules returned a true result.
/// If a rule returns a false result, the Default rule will be returned.
/// </remarks>
public enum EvaluationTypeEnum
{
    FIRST = 0,
    ANY = 1,
    ALL = 2
}
