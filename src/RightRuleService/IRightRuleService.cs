using RightRule.LanguageProcessor.SyntaxChecker;

namespace RightRule.RightRuleService;

internal interface IRightRuleService
{
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);
}
