using Microsoft.Extensions.Logging.Abstractions;
using RightRule.DataServices;
using RightRule.DataServices.DataValidator;
using RightRule.Exceptions;
using RightRule.Factories.TokenizerFactory;
using RightRule.LanguageProcessor.Tokenizer;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;

namespace RightRuleTests;

public sealed class TokenizerFactoryTests
{
    private static CustomOperatorLoader CreateLoader() =>
        new(NullLogger<CustomOperatorLoader>.Instance);

    [Fact]
    public void BuildTokenizer_PassesStatementAndStartingPosition()
    {
        var factory = CreateFactory(dataCheck: false);

        var token = factory.BuildTokenizer("  123", 0).NextToken();

        Assert.Equal(TokenTypeEnum.NUMBER, token.Type);
        Assert.Equal("123", token.Value);
        Assert.Equal(2, token.Position);
    }

    [Fact]
    public void BuildTokenizer_WhenDataCheckIsDisabled_AllowsUnknownVariable()
    {
        var factory = CreateFactory(dataCheck: false);

        var token = factory.BuildTokenizer("Missing", 0).NextToken();

        Assert.Equal(TokenTypeEnum.VARIABLE, token.Type);
        Assert.Equal("Missing", token.Value);
    }

    [Fact]
    public void BuildTokenizer_WhenDataCheckIsEnabled_RejectsUnknownVariable()
    {
        var factory = CreateFactory(dataCheck: true);

        var exception = Assert.Throws<InvalidIdentifierException>(
            () => factory.BuildTokenizer("Missing", 0).NextToken());

        Assert.Equal("Missing", exception.Identifier);
        Assert.Equal(0, exception.Position);
    }

    private static TokenizerFactory CreateFactory(bool dataCheck)
    {
        var options = new RightRuleOptions
        {
            DataCheck = dataCheck,
            LoadDefaultOperators = false,
            LoadCustomOperators = false
        };

        var operatorService = new OperatorService(CreateLoader(), options);
        var dataValidator = new DataValidatorService();
        var dataService = new DataService(
            NullLogger<DataService>.Instance,
            dataValidator);

        return new TokenizerFactory(
            operatorService,
            dataService,
            dataValidator,
            options);
    }
}