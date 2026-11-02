using System.Reflection;
using RightRule.Factories.ParserFactory;
using RightRule.Factories.TokenizerFactory;
using RightRule.LanguageProcessor.Parser;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.System.Common.Operator.Models;
using Xunit;

namespace RightRuleTests;

public sealed class StatementSyntaxCheckerTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CheckAsync_EmptyStatement_ReturnsEmptyStatementDiagnostic(string statement)
    {
        var result = await CreateChecker(statement).CheckAsync(statement);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("Statement is empty.", diagnostic.Message);
        Assert.Equal(0, diagnostic.Position);
        Assert.Equal(0, diagnostic.Length);
        Assert.Equal(string.Empty, diagnostic.Snippet);
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task CheckAsync_UppercaseLogicalKeyword_ReturnsCaseDiagnostic()
    {
        const string statement = "field equals value AND other equals value";
        var tokenizer = new StubTokenizer(
            new Token(TokenTypeEnum.IDENTIFIER, "field", 0),
            new Token(TokenTypeEnum.OPERATOR, "equals", 6),
            new Token(TokenTypeEnum.IDENTIFIER, "value", 13),
            new Token(TokenTypeEnum.IDENTIFIER, "AND", 19),
            new Token(TokenTypeEnum.IDENTIFIER, "other", 23));
        var checker = CreateChecker(statement, tokenizer);

        var result = await checker.CheckAsync(statement);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("LOGIC_OPERATOR_CASE", diagnostic.Code);
        Assert.Equal(19, diagnostic.Position);
        Assert.Equal(3, diagnostic.Length);
        Assert.Contains("must be lowercase", diagnostic.Message);
        Assert.Equal("Replace 'AND' with 'and'.", diagnostic.Suggestion);
    }

    [Fact]
    public async Task CheckAsync_UnmatchedClosingParenthesis_ReturnsParenthesisDiagnostic()
    {
        const string statement = ")";
        var checker = CreateChecker(
            statement,
            new StubTokenizer(new Token(TokenTypeEnum.RPAREN, ")", 0)));

        var result = await checker.CheckAsync(statement);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("PAR_UNMATCHED_RPAREN", diagnostic.Code);
        Assert.Equal(0, diagnostic.Position);
        Assert.Equal(1, diagnostic.Length);
    }

    [Fact]
    public async Task CheckAsync_UnmatchedOpeningParenthesis_ReturnsParenthesisDiagnostic()
    {
        const string statement = "(";
        var checker = CreateChecker(
            statement,
            new StubTokenizer(new Token(TokenTypeEnum.LPAREN, "(", 0)));

        var result = await checker.CheckAsync(statement);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("PAR_UNMATCHED_LPAREN", diagnostic.Code);
        Assert.Equal(0, diagnostic.Position);
        Assert.Equal(1, diagnostic.Length);
    }

    [Fact]
    public async Task CheckAsync_TokenizerThrows_ReturnsTokenizerDiagnostic()
    {
        const string statement = "field";
        var checker = CreateChecker(statement, new ThrowingTokenizer("Unexpected character"));

        var result = await checker.CheckAsync(statement);

        var diagnostic = Assert.Single(result.Diagnostics);
        Assert.Equal("TKN", diagnostic.Code);
        Assert.Equal("Unexpected character", diagnostic.Message);
        Assert.Equal("Remove or escape the unexpected character.", diagnostic.Suggestion);
    }

    [Fact]
    public async Task CheckAsync_ParserReportsExpectedOperator_ReturnsParserErrorAndHint()
    {
        const string statement = "field value";
        var checker = CreateChecker(
            statement,
            new StubTokenizer(new Token(TokenTypeEnum.IDENTIFIER, "field", 0)),
            new StubParser(() => throw new InvalidOperationException("Expected operator")));

        var result = await checker.CheckAsync(statement);

        Assert.Collection(
            result.Diagnostics,
            error =>
            {
                Assert.Equal("PRS", error.Code);
                Assert.Equal("Expected operator", error.Message);
            },
            hint =>
            {
                Assert.Equal("HINT_OPERATOR", hint.Code);
                Assert.Equal(DiagnosticSeverityEnum.INFO, hint.Severity);
            });
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task CheckAsync_NoDiagnostics_ReturnsValidResult()
    {
        const string statement = "field equals value";
        var checker = CreateChecker(
            statement,
            new StubTokenizer(
                new Token(TokenTypeEnum.IDENTIFIER, "field", 0),
                new Token(TokenTypeEnum.OPERATOR, "equals", 6),
                new Token(TokenTypeEnum.IDENTIFIER, "value", 13)));

        var result = await checker.CheckAsync(statement);

        Assert.True(result.IsValid);
        Assert.Empty(result.Diagnostics);
    }

    private static StatementSyntaxChecker CreateChecker(
        string statement,
        ITokenizer? tokenizer = null,
        IParser? parser = null)
    {
        var parserFactory = new StubParserFactory(parser ?? new StubParser(() => null!));
        var tokenizerFactory = new StubTokenizerFactory(
            statement,
            tokenizer ?? new StubTokenizer());
        var operatorService = DispatchProxy.Create<IOperatorService, EmptyOperatorServiceProxy>();

        return new StatementSyntaxChecker(
            operatorService,
            parserFactory,
            tokenizerFactory,
            new RightRuleOptions());
    }

    private sealed class StubParserFactory(IParser parser) : IParserFactory
    {
        public IParser BuildParser(string Statement) => parser;

        public IParser BuildParser(RuleDefinition ruleDefinition) => parser;
    }

    private sealed class StubTokenizerFactory(string statement, ITokenizer tokenizer) : ITokenizerFactory
    {
        public ITokenizer BuildTokenizer(string requestedStatement, int pos) =>
            requestedStatement == statement ? tokenizer : new StubTokenizer();
    }

    private sealed class StubParser(Func<Node> parse) : IParser
    {
        public Node Parse() => parse();
    }

    private sealed class StubTokenizer(params Token[] tokens) : ITokenizer
    {
        private int _index;

        public Token NextToken() =>
            _index < tokens.Length
                ? tokens[_index++]
                : new Token(TokenTypeEnum.END, string.Empty);
    }

    private sealed class ThrowingTokenizer(string message) : ITokenizer
    {
        public Token NextToken() => throw new InvalidOperationException(message);
    }

    public class EmptyOperatorServiceProxy : DispatchProxy
    {
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) =>
            targetMethod?.Name switch
            {
                "get_OperatorAliases" => new Dictionary<string, Operator>(),
                _ => null
            };
    }
}