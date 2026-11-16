using RightRule.LanguageProcessor.SyntaxChecker;

namespace RightRule.RightRuleService;

public interface IRightRuleService
{
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);
}
