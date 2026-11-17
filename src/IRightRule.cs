using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RuleServices;

namespace RightRule;

public interface IRightRule : IDisposable, IAsyncDisposable
{
    IRuleService RuleService { get; }
    IDataService DataService { get; }
    IOperatorService? OperatorService { get; }
    IDataAdaptorService? DataAdaptorService { get; }

    RightRuleOptions RightRuleOptions { get; }

    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see cref="CheckStatementResult"/></returns>
    CheckStatementResult StatementSyntaxCheck(string Statement);

    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see cref="CheckStatementResult"/></returns>
    Task<CheckStatementResult> StatementSyntaxCheckAsync(string Statement);
}
