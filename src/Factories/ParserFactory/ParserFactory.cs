using RightRule.Factories.TokenizerFactory;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.Models.BaseModels;

namespace RightRule.Factories.ParserFactory
{
    internal class ParserFactory : IParserFactory
    {
        private readonly ITokenizerFactory _tokenizerFactory;

        public ParserFactory(ITokenizerFactory tokenizerFactory)
        {
            _tokenizerFactory = tokenizerFactory;
        }

        public IParser BuildParser(string Statement, bool DataCheck)
        {
            int initialPos = 0;
            return BuildParser(Statement, DataCheck, initialPos);
        }

        public IParser BuildParser(RuleDefinition ruleDefinition)
        {
            string statement = ruleDefinition.Statement;
            bool dataCheck = ruleDefinition.RuleConfiguration.DataCheck;
            int initialPos = 0;
            return BuildParser(statement, dataCheck, initialPos);
        }

        private IParser BuildParser(string Statement, bool DataCheck, int InitialPos)
        {
            var tokenizer = BuildTokenizer(Statement, InitialPos, DataCheck);
            return new Parser(tokenizer, Statement, DataCheck);
        }

        private ITokenizer BuildTokenizer(string statement, int initialPos, bool dataCheck)
        {
            return _tokenizerFactory.BuildTokenizer(statement, initialPos, dataCheck);
        }
    }
}
