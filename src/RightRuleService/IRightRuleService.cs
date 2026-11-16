using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;

namespace RightRule.RightRuleService;

public interface IRightRuleService
{
    IOperatorService? OperatorService { get; }
    
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);
}
