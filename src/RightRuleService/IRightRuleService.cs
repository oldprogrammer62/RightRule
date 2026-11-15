using RightRule.DataAdaptors;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.System.Common.DataAdaptor;

namespace RightRule.RightRuleService;

public interface IRightRuleService
{
    IOperatorService? OperatorService { get; }
    IDataAdaptorService? DataAdaptorService { get; }

    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor dataAdaptor);
}
