using RightRule.Factories.TokenizerFactory;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class ParserFactoryTests
{
    [Fact]
    public void BuildParser_WithStatement_PassesStatementAndInitialPositionToTokenizerFactory()
    {
        const string statement = "Customer.Age greaterthan 18";
        var tokenizerFactory = new RecordingTokenizerFactory();
        var sut = new RightRule.Factories.ParserFactory.ParserFactory(tokenizerFactory);

        var parser = sut.BuildParser(statement);

        Assert.IsType<Parser>(parser);
        Assert.Equal(statement, tokenizerFactory.Statement);
        Assert.Equal(0, tokenizerFactory.Position);
        Assert.Equal(1, tokenizerFactory.CallCount);
    }

    [Fact]
    public void BuildParser_WithRuleDefinition_PassesRuleStatementAndInitialPositionToTokenizerFactory()
    {
        const string statement = "Customer.Age greaterthan 18";
        var ruleDefinition = new DefinedRule
        {
            Number = 1,
            Name = "Adult customer",
            Context = "Customer",
            Statement = statement,
            Decision = "Eligible",
            FalseDecision = "Not eligible"
        };
        var tokenizerFactory = new RecordingTokenizerFactory();
        var sut = new RightRule.Factories.ParserFactory.ParserFactory(tokenizerFactory);

        var parser = sut.BuildParser(ruleDefinition);

        Assert.IsType<Parser>(parser);
        Assert.Equal(statement, tokenizerFactory.Statement);
        Assert.Equal(0, tokenizerFactory.Position);
        Assert.Equal(1, tokenizerFactory.CallCount);
    }

    private sealed class RecordingTokenizerFactory : ITokenizerFactory
    {
        public string? Statement { get; private set; }
        public int Position { get; private set; }
        public int CallCount { get; private set; }

        public ITokenizer BuildTokenizer(string statement, int pos)
        {
            Statement = statement;
            Position = pos;
            CallCount++;

            return new StubTokenizer();
        }
    }

    private sealed class StubTokenizer : ITokenizer
    {
        public Token NextToken() =>
            throw new InvalidOperationException("The tokenizer should not be called while building a parser.");
    }
}