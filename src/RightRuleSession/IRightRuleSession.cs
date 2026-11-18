using Microsoft.Extensions.Logging;
using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RuleServices;

namespace RightRule.RightRuleSession;

public interface IRightRuleSession : IDisposable, IAsyncDisposable
{
    IRuleService RuleService { get; }
    IDataService DataService { get; }
    IDataAdaptorService DataAdaptorService { get; }
    IOperatorService? OperatorService { get; }
    RightRuleOptions RightRuleOptions { get; }
    IServiceProvider ServiceProvider { get; }
    ILogger<RightRuleSession> Logger { get; }

    CheckStatementResult StatementSyntaxCheck(string Statement);
    Task<CheckStatementResult> StatementSyntaxCheckAsync(string Statement);
}
