using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.RuleBuilders.RightRuleBuilder;
using RightRule.System.Common.DataAdaptor;

namespace RightRule;

public interface IRightRule : IDisposable
{
    RightRuleOptions GetRightRuleOptions();

    IRuleService RuleService { get; }
    IDataService DataService { get; }
    IOperatorService? OperatorService { get; }
    IDataAdaptorService? DataAdaptorService { get; }

    RightRuleOptions Options { get; }

    /// <summary>
    /// Syntax check a Rule Statement to ensure it is valid and can be parsed.
    /// </summary>
    /// <param name="Statement">The rule statement to check.</param>
    /// <returns>The result of the syntax check.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see cref="CheckStatementResult"/></returns>
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    /// <summary>
    /// Registers a custom Data Adaptor with the RightRule engine.<br/>
    /// </summary>
    /// <param name="DataAdaptor"></param>
    /// <returns><see cref="DataAdaptorRegistrationResult"/></returns>
    DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor DataAdaptor);
}
