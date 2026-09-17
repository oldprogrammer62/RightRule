namespace RightRule.LanguageProcessor.SyntaxChecker
{
    internal interface IStatementSyntaxChecker
    {
        Task<CheckStatementResult> CheckAsync(string Statement, bool DataCheck);
    }
}