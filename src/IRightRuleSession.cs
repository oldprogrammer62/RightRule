using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RuleServices;

namespace RightRule;

public interface IRightRuleSession : IDisposable, IAsyncDisposable
{
    IRuleService RuleService { get; }
    IDataService DataService { get; }
    IDataAdaptorService DataAdaptorService { get; }
    IOperatorService? OperatorService { get; }
    RightRuleOptions RightRuleOptions { get; }

    CheckStatementResult StatementSyntaxCheck(string Statement);
    Task<CheckStatementResult> StatementSyntaxCheckAsync(string Statement);
}
