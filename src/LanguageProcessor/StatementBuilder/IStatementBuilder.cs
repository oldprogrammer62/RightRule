using RightRule.LanguageProcessor.Parser;

namespace RightRule.LanguageProcessor.StatementBuilder
{
    public interface IStatementBuilder
    {
        Task<string> BuildAppliedStatement(Node? Node, string Statement);
    }
}