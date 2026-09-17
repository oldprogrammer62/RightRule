namespace RightRule.SyntaxChecker
{
    internal interface IStatementSyntaxChecker
    {
        Task<CheckStatementResult> CheckAsync(string Statement, bool DataCheck);
    }
}