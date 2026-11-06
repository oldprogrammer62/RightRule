using Microsoft.Extensions.Logging.Abstractions;
using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.Exceptions;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using RightRule.OperatorServices.OperatorLoader.DefaultOperators;

namespace RightRuleTests;

public sealed class TokenizerTests
{
    private static CustomOperatorLoader CreateCustomLoader() =>
        new(NullLogger<CustomOperatorLoader>.Instance);

    private static DefaultOperatorLoader CreateDefaultLoader() =>
        new(NullLogger<DefaultOperatorLoader>.Instance);

    [Fact]
    public void NextToken_ReturnsEndForEmptyOrWhitespaceInput()
    {
        var tokenizer = CreateTokenizer(" \t ");

        var token = tokenizer.NextToken();

        Assert.Equal(TokenTypeEnum.END, token.Type);
        Assert.Equal(string.Empty, token.Value);
        Assert.Equal(3, token.Position);
    }

    [Fact]
    public void NextToken_SkipsWhitespaceAndTracksPositions()
    {
        var tokenizer = CreateTokenizer(" \t( abc ) ");

        var leftParenthesis = tokenizer.NextToken();
        var variable = tokenizer.NextToken();
        var rightParenthesis = tokenizer.NextToken();
        var end = tokenizer.NextToken();

        Assert.Equal((TokenTypeEnum.LPAREN, "(", 2),
            (leftParenthesis.Type, leftParenthesis.Value, leftParenthesis.Position));
        Assert.Equal((TokenTypeEnum.VARIABLE, "abc", 4),
            (variable.Type, variable.Value, variable.Position));
        Assert.Equal((TokenTypeEnum.RPAREN, ")", 8),
            (rightParenthesis.Type, rightParenthesis.Value, rightParenthesis.Position));
        Assert.Equal((TokenTypeEnum.END, string.Empty, 10),
            (end.Type, end.Value, end.Position));
    }

    [Theory]
    [InlineData("123", "123")]
    [InlineData("12.5", "12.5")]
    public void NextToken_ReadsNumbers(string statement, string expectedValue)
    {
        var token = CreateTokenizer(statement).NextToken();

        Assert.Equal(TokenTypeEnum.NUMBER, token.Type);
        Assert.Equal(expectedValue, token.Value);
        Assert.Equal(0, token.Position);
    }

    [Theory]
    [InlineData("and", TokenTypeEnum.AND)]
    [InlineData("or", TokenTypeEnum.OR)]
    [InlineData("not", TokenTypeEnum.NOT)]
    [InlineData("true", TokenTypeEnum.BOOLEAN)]
    [InlineData("false", TokenTypeEnum.BOOLEAN)]
    public void NextToken_RecognizesLowercaseKeywords(string statement, TokenTypeEnum expectedType)
    {
        var token = CreateTokenizer(statement).NextToken();

        Assert.Equal(expectedType, token.Type);
        Assert.Equal(statement, token.Value);
    }

    [Theory]
    [InlineData("AND")]
    [InlineData("Or")]
    [InlineData("NOT")]
    [InlineData("True")]
    [InlineData("FALSE")]
    public void NextToken_RejectsKeywordsWithIncorrectCase(string statement)
    {
        Assert.Throws<InvalidIdentifierException>(
            () => CreateTokenizer(statement).NextToken());
    }

    [Theory]
    [InlineData("_customer9", TokenTypeEnum.VARIABLE)]
    [InlineData("customer.age", TokenTypeEnum.IDENTIFIER)]
    public void NextToken_ReadsIdentifiers(string statement, TokenTypeEnum expectedType)
    {
        var token = CreateTokenizer(statement).NextToken();

        Assert.Equal(expectedType, token.Type);
        Assert.Equal(statement, token.Value);
        Assert.Equal(0, token.Position);
    }

    [Theory]
    [InlineData("customer..age")]
    [InlineData("customer.age.total")]
    public void NextToken_RejectsIdentifiersWithMultipleDots(string statement)
    {
        Assert.Throws<InvalidIdentifierException>(
            () => CreateTokenizer(statement).NextToken());
    }

    [Fact]
    public void NextToken_RecognizesRegisteredOperatorAliases()
    {
        var operatorService = CreateOperatorService();
        operatorService.OperatorAliases["equals"] = null!;

        var token = CreateTokenizer("equals", operatorService: operatorService).NextToken();

        Assert.Equal(TokenTypeEnum.OPERATOR, token.Type);
        Assert.Equal("equals", token.Value);
    }

    [Fact]
    public void NextToken_ReadsQuotedStringsAndUnescapesSupportedCharacters()
    {
        var tokenizer = CreateTokenizer("'a\\\"b' \"c\\\\d\"");

        var singleQuoted = tokenizer.NextToken();
        var doubleQuoted = tokenizer.NextToken();

        Assert.Equal(TokenTypeEnum.STRING, singleQuoted.Type);
        Assert.Equal("a\"b", singleQuoted.Value);
        Assert.Equal(TokenTypeEnum.STRING, doubleQuoted.Type);
        Assert.Equal("c\\d", doubleQuoted.Value);
    }

    [Theory]
    [InlineData("'unterminated")]
    [InlineData("\"unterminated")]
    public void NextToken_RejectsUnterminatedStrings(string statement)
    {
        Assert.Throws<Exception>(() => CreateTokenizer(statement).NextToken());
    }

    [Fact]
    public void NextToken_ReadsArrayLiteral()
    {
        var token = CreateTokenizer("[1, 2.5]").NextToken();

        Assert.Equal(TokenTypeEnum.ARRAY, token.Type);
        Assert.Equal("[1, 2.5]", token.Value);
        Assert.Equal(0, token.Position);
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("[1,]")]
    [InlineData("[[1, 2]]")]
    [InlineData("[1, 'two']")]
    public void NextToken_RejectsInvalidArrayLiterals(string statement)
    {
        Assert.Throws<FormatException>(() => CreateTokenizer(statement).NextToken());
    }

    [Fact]
    public void NextToken_RejectsUnterminatedArrayLiteral()
    {
        Assert.Throws<FormatException>(() => CreateTokenizer("[1, 2").NextToken());
    }

    [Fact]
    public void NextToken_RejectsUnrecognizedSymbol()
    {
        Assert.Throws<FormatException>(() => CreateTokenizer("@").NextToken());
    }

    [Fact]
    public void NextToken_ValidatesDatasetIdentifiersWhenDataCheckIsEnabled()
    {
        var validator = new DataValidatorService();
        var dataService = new DataService(NullLogger<DataService>.Instance, validator);
        dataService.AddData("customer", ("age", (object)42));

        var token = CreateTokenizer(
            "customer.age",
            dataCheck: true,
            dataService: dataService,
            dataValidator: validator).NextToken();

        Assert.Equal(TokenTypeEnum.IDENTIFIER, token.Type);
        Assert.Equal("customer.age", token.Value);
    }

    [Fact]
    public void NextToken_RejectsUnknownDatasetIdentifiersWhenDataCheckIsEnabled()
    {
        var validator = new DataValidatorService();
        var dataService = new DataService(NullLogger<DataService>.Instance, validator);

        Assert.Throws<InvalidIdentifierException>(() =>
            CreateTokenizer(
                "customer.age",
                dataCheck: true,
                dataService: dataService,
                dataValidator: validator).NextToken());
    }

    private static Tokenizer CreateTokenizer(
        string statement,
        bool dataCheck = false,
        IDataService? dataService = null,
        IDataValidatorService? dataValidator = null,
        OperatorService? operatorService = null)
    {
        return new Tokenizer(
            operatorService ?? CreateOperatorService(),
            dataService!,
            dataValidator!,
            statement,
            0,
            dataCheck);
    }

    private static OperatorService CreateOperatorService()
    {
        return new OperatorService(CreateDefaultLoader(), CreateCustomLoader(), new RightRuleOptions
        {
            LoadDefaultOperators = false,
            LoadCustomOperators = false
        });
    }
}