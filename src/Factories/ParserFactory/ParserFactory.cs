using RightRule.Factories.TokenizerFactory;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule.Factories.ParserFactory;

internal class ParserFactory : IParserFactory
{
    private readonly ITokenizerFactory _tokenizerFactory;

    public ParserFactory(ITokenizerFactory tokenizerFactory)
    {
        _tokenizerFactory = tokenizerFactory;
    }

    public IParser BuildParser(string Statement)
    {
        int initialPos = 0;
        return BuildParser(Statement, initialPos);
    }

    public IParser BuildParser(RuleDefinition ruleDefinition)
    {
        string statement = ruleDefinition.Statement;
        int initialPos = 0;
        return BuildParser(statement, initialPos);
    }

    private IParser BuildParser(string Statement, int InitialPos)
    {
        var tokenizer = BuildTokenizer(Statement, InitialPos);
        return new Parser(tokenizer, Statement);
    }

    private ITokenizer BuildTokenizer(string Statement, int InitialPos)
    {
        return _tokenizerFactory.BuildTokenizer(Statement, InitialPos);
    }
}
