using RightRule.LanguageProcessor.Parser;

namespace RightRule.LanguageProcessor.StatementBuilder
{
    public interface IStatementBuilder
    {
        Task<string> BuildAppliedStatement(Node? node, string originalStatement);
    }
}